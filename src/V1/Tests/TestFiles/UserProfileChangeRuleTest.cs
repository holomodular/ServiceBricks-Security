using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract partial class UserProfileChangeRuleTestBase : UserConfirmEmailRuleTestBase
    {
        protected override void CleanupDependencies()
        {
            //var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            //audituserservice.Delete(ProfileChangeAuditStorageKey);

            base.CleanupDependencies();
        }

        public string ProfileChangeAuditStorageKey { get; set; }

        [Fact]
        public override async Task TestRule()
        {
            await base.TestRule();

            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();
            var timezoneService = SystemManager.ServiceProvider.GetRequiredService<ITimezoneService>();

            // Execute ApplicationUserProfileChangeProcess
            UserProfileChangeProcess changeProcess = new UserProfileChangeProcess(
                UserStorageKey);
            var respChange = await ruleService.ExecuteProcessAsync(changeProcess);
            Assert.True(respChange.Success);

            // Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.PROFILE_CHANGE_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            ProfileChangeAuditStorageKey = respAudit.Item.List[0].StorageKey;

            // Verify user properties changed
            var userService = SystemManager.ServiceProvider.GetRequiredService<IUserApiService>();
            var respUser = await userService.GetAsync(UserStorageKey.ToString());
            Assert.True(respUser != null);
            Assert.True(respUser.Item != null);
        }
    }
}