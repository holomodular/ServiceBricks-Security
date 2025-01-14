using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class UserRegisterAdminRuleTestBase : IAsyncDisposable
    {
        public UserRegisterAdminRuleTestBase()
        {
        }

        public async ValueTask DisposeAsync()
        {
            await CleanupDependencies();
        }

        protected virtual async Task CleanupDependencies()
        {
            string userstoragekey = UserStorageKey;

            // Cleanup Audits
            var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var auditquery = new ServiceQueryRequestBuilder().IsEqual(nameof(UserAuditDto.UserStorageKey), userstoragekey).Build();
            var respAudits = await audituserservice.QueryAsync(auditquery);
            foreach (var item in respAudits.Item.List)
                await audituserservice.DeleteAsync(item.StorageKey);

            // Cleanup UserRoles
            var userroleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserRoleDto>>();
            var roleq = new ServiceQueryRequestBuilder().IsEqual(nameof(UserRoleDto.UserStorageKey), userstoragekey).Build();
            var respUserRoles = await userroleservice.QueryAsync(roleq);
            foreach (var item in respUserRoles.Item.List)
                await userroleservice.DeleteAsync(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserDto>>();
            await userservice.DeleteAsync(userstoragekey);
        }

        public ISystemManager SystemManager { get; set; }

        public string UserStorageKey { get; set; }

        [Fact]
        public async Task TestRule()
        {
            string email = Guid.NewGuid().ToString().ToUpper() + SecurityTestConstants.EMAIL_ATSUFFIX;
            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            UserRegisterAdminProcess process = new UserRegisterAdminProcess(
                email,
                SecurityTestConstants.PASSWORD);
            var resp = await ruleService.ExecuteProcessAsync(process);
            Assert.True(resp.Success);

            // Verify user found
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByEmailAsync(email);
            var user = respUser.Item;
            Assert.True(user != null);
            UserStorageKey = user.StorageKey;

            // Verify roles
            var roleApiService = SystemManager.ServiceProvider.GetRequiredService<IRoleApiService>();
            var qb = ServiceQueryRequestBuilder.New().IsEqual(nameof(RoleDto.Name), ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME);
            var respUserRole = await roleApiService.QueryAsync(qb.Build());
            Assert.True(respUserRole.Item.List != null && respUserRole.Item.List.Count == 1);
            if (respUserRole.Item.List == null)
                throw new Exception("list null");
            var userRole = respUserRole.Item.List[0];

            var userRoleApiService = SystemManager.ServiceProvider.GetRequiredService<IUserRoleApiService>();
            qb = ServiceQueryRequestBuilder.New().IsEqual(nameof(UserRoleDto.UserStorageKey), user.StorageKey);
            var respRoles = await userRoleApiService.QueryAsync(qb.Build());
            Assert.True(respRoles.Item.List != null && respRoles.Item.List.Count > 0);
            Assert.True(respRoles.Item.List.Where(x => x.RoleStorageKey == userRole.StorageKey).Any());

            //Verify notification created
        }
    }
}