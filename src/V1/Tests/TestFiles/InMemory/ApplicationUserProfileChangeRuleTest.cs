using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserProfileChangeRuleTest : ApplicationUserConfirmEmailRuleTest
    {
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
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.PROFILE_CHANGE);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);

            // Verify user properties changed
            var userService = SystemManager.ServiceProvider.GetRequiredService<IApplicationUserApiService>();
            var respUser = await userService.GetAsync(UserStorageKey.ToString());
            Assert.True(respUser != null);
            Assert.True(respUser.Item != null);
        }
    }
}