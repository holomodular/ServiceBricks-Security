using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract partial class UserPasswordResetRuleTestBase : UserForgotPasswordRuleTestBase
    {
        public string PasswordResetAuditStorageKey { get; set; }

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
            var userService = SystemManager.ServiceProvider.GetRequiredService<IUserApiService>();
            respUser = await userService.GetAsync(UserStorageKey.ToString());
            Assert.True(respUser != null);
            Assert.True(respUser.Item != null);
            Assert.True(respUser.Item.EmailConfirmed == true);

            //Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.PASSWORD_RESET_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            PasswordResetAuditStorageKey = respAudit.Item.List[0].StorageKey;
        }
    }
}