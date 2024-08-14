using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract partial class ApplicationUserLogoutRuleTestBase : ApplicationUserConfirmEmailRuleTestBase
    {
        protected override void CleanupDependencies()
        {
            //var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            //audituserservice.Delete(LogoutAuditStorageKey);
            //audituserservice.Delete(LoginAuditStorageKey);

            base.CleanupDependencies();
        }

        public string LoginAuditStorageKey { get; set; }
        public string LogoutAuditStorageKey { get; set; }

        [Fact]
        public override async Task TestRule()
        {
            await base.TestRule();

            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Create httpcontext
            var httpContextAccessor = SystemManager.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.RequestServices = SystemManager.ServiceProvider;

            //Execute ApplicationUserLoginEvent
            UserLoginProcess loginProcess = new UserLoginProcess(
                Email,
                Password,
                true);
            var respLogin = await ruleService.ExecuteProcessAsync(loginProcess);
            Assert.True(respLogin.Success);

            //Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.LOGIN_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            LoginAuditStorageKey = respAudit.Item.List[0].StorageKey;

            //Execute ApplicationUserLogoutProcess
            UserLogoutProcess logoutProcess = new UserLogoutProcess(
                UserStorageKey);
            var respLogout = await ruleService.ExecuteProcessAsync(logoutProcess);
            Assert.True(respLogout.Success);

            //Verify audituser created
            auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.LOGOUT_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), UserStorageKey.ToString());
            respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            LogoutAuditStorageKey = respAudit.Item.List[0].StorageKey;
        }
    }
}