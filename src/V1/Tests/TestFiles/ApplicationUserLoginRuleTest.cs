using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract partial class ApplicationUserLoginRuleTestBase : ApplicationUserConfirmEmailRuleTestBase
    {
        protected override void CleanupDependencies()
        {
            //var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            //audituserservice.Delete(LoginAuditStorageKey);

            base.CleanupDependencies();
        }

        public virtual string LoginAuditStorageKey { get; set; }

        [Fact]
        public override async Task TestRule()
        {
            await base.TestRule();

            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Execute ApplicationUserLoginProcess
            UserLoginProcess loginProcess = new UserLoginProcess(
                Email,
                Password,
                true);

            // Login will fail with no httpcontext
            var respLogin = await ruleService.ExecuteProcessAsync(loginProcess);
            Assert.True(respLogin.Error &&
                respLogin.Messages.Count == 1 &&
                respLogin.Messages.First().Message == LocalizationResource.UNIT_TEST);

            // Create httpcontext
            var httpContextAccessor = SystemManager.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.RequestServices = SystemManager.ServiceProvider;

            // Login again with new context
            respLogin = await ruleService.ExecuteProcessAsync(loginProcess);
            Assert.True(respLogin.Success);

            // Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.LOGIN);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            LoginAuditStorageKey = respAudit.Item.List[0].StorageKey;
        }
    }
}