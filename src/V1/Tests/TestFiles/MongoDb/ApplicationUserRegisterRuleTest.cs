using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;
using System.Security.Claims;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRegisterRuleTest
    {
        public ApplicationUserRegisterRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
        }

        public ISystemManager SystemManager { get; set; }

        [Fact]
        public async Task TestRule()
        {
            string email = Guid.NewGuid().ToString().ToUpper() + SecurityTestConstants.EMAIL_ATSUFFIX;
            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            UserRegisterProcess process = new UserRegisterProcess(
                email,
                SecurityTestConstants.PASSWORD);
            var resp = await ruleService.ExecuteProcessAsync(process);
            Assert.True(resp.Success);

            // Verify user found
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByEmailAsync(email);
            var user = respUser.Item;
            Assert.True(user != null);

            // Verify roles
            var roleApiService = SystemManager.ServiceProvider.GetRequiredService<IApplicationRoleApiService>();
            var qb = ServiceQueryRequestBuilder.New().IsEqual(nameof(ApplicationRoleDto.Name), SecurityConstants.ROLE_USER_NAME);
            var respUserRole = await roleApiService.QueryAsync(qb.Build());
            Assert.True(respUserRole.Item.List != null && respUserRole.Item.List.Count == 1);
            if (respUserRole.Item.List == null)
                throw new Exception("list null");
            var userRole = respUserRole.Item.List[0];

            var userRoleApiService = SystemManager.ServiceProvider.GetRequiredService<IApplicationUserRoleApiService>();
            qb = ServiceQueryRequestBuilder.New().IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), user.StorageKey);
            var respRoles = await userRoleApiService.QueryAsync(qb.Build());
            Assert.True(respRoles.Item.List != null && respRoles.Item.List.Count > 0);
            Assert.True(respRoles.Item.List.Where(x => x.RoleStorageKey == userRole.StorageKey).Any());

            // Verify claims
            //var userClaimApiService = SystemManager.ServiceProvider.GetRequiredService<IApplicationUserClaimApiService>();

            //var respUserClaims = await userClaimApiService.QueryAsync(qb.Build());
            //Assert.True(respUserClaims.Success && respUserClaims.List != null && respUserClaims.List.Count > 0);
            //var claims = respUserClaims.List;
            //Assert.True(claims.Where(x => x.ClaimType == ClaimTypes.Email).Count() == 1);
            //Assert.True(claims.Where(x => x.ClaimType == TimezoneService.CLAIM_TIMEZONE).Count() == 1);

            // Verify notification created

            // Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.REGISTER);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), user.StorageKey);
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
        }
    }
}