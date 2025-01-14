using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract partial class UserPasswordChangeRuleTestBase : UserConfirmEmailRuleTestBase
    {
        public string LoginAuditStorageKey { get; set; }
        public string PasswordChangedAuditStorageKey { get; set; }

        [Fact]
        public override async Task TestRule()
        {
            await base.TestRule();

            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Create httpcontext
            var httpContextAccessor = SystemManager.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.RequestServices = SystemManager.ServiceProvider;

            //Execute ApplicationUserLoginProcess
            UserLoginProcess loginProcess = new UserLoginProcess(
                Email,
                Password,
                true);
            var respLogin = await ruleService.ExecuteProcessAsync(loginProcess);
            Assert.True(respLogin.Success);

            //Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.LOGIN_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            LoginAuditStorageKey = respAudit.Item.List[0].StorageKey;

            //Execute ApplicationUserPasswordChangeProcess
            UserPasswordChangeProcess changeProcess = new UserPasswordChangeProcess(
                UserStorageKey.ToString(),
                SecurityTestConstants.PASSWORD,
                SecurityTestConstants.PASSWORD + "1");
            var respChange = await ruleService.ExecuteProcessAsync(changeProcess);
            Assert.True(respChange.Success);

            //Verify audituser created
            auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.PASSWORD_CHANGE_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), UserStorageKey.ToString());
            respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            PasswordChangedAuditStorageKey = respAudit.Item.List[0].StorageKey;
        }
    }
}