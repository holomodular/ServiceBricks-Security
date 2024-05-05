using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserPasswordResetRuleTest : ApplicationUserForgotPasswordRuleTest
    {
        [Fact]
        public override async Task TestRule()
        {
            await base.TestRule();

            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            //Find User
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByIdAsync(UserStorageKey.ToString());
            Assert.True(respUser.Item != null);

            //Find token from notification
            var respCode = await userManager.GeneratePasswordResetTokenAsync(respUser.Item.StorageKey);

            //Execute ApplicationUserPasswordResetProcess
            UserPasswordResetProcess resetProcess = new UserPasswordResetProcess(
                respUser.Item.Email,
                SecurityTestConstants.PASSWORD,
                respCode.Item);
            var respReset = await ruleService.ExecuteProcessAsync(resetProcess);
            Assert.True(respReset.Success);

            //Verify user email confirmed
            var userService = SystemManager.ServiceProvider.GetRequiredService<IApplicationUserApiService>();
            respUser = await userService.GetAsync(UserStorageKey.ToString());
            Assert.True(respUser != null);
            Assert.True(respUser.Item != null);
            Assert.True(respUser.Item.EmailConfirmed == true);

            //Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.PASSWORD_RESET);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
        }
    }
}