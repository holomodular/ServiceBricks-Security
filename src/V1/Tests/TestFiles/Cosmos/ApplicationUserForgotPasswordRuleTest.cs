using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserForgotPasswordRuleTest //: IDisposable
    {
        public ApplicationUserForgotPasswordRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
        }

        public ISystemManager SystemManager { get; set; }

        //public virtual void Dispose()
        //{
        //    SystemManager?.StopSystem();
        //}

        public virtual string UserStorageKey { get; set; }

        [Fact]
        public virtual async Task TestRule()
        {
            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Execute ApplicationUserRegisterEvent
            string email = Guid.NewGuid().ToString().ToUpper() + SecurityTestConstants.EMAIL_ATSUFFIX;
            UserRegisterProcess registerProcess = new UserRegisterProcess(
                email,
                SecurityTestConstants.PASSWORD);

            var respRegister = await ruleService.ExecuteProcessAsync(registerProcess);
            var reg = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var list = reg.GetRegistryList(typeof(UserRegisterProcess));
            if (respRegister.Error)
            {
                string er = list.Count.ToString() + " " + respRegister.ToString();
                throw new Exception(er);
            }
            Assert.True(respRegister.Success);

            // Find User
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByEmailAsync(email);
            Assert.True(respUser.Item != null);
            var user = respUser.Item;
            UserStorageKey = user.StorageKey;

            // Execute ApplicationUserForgotPasswordProcess
            UserForgotPasswordProcess forgotPasswordProcess = new UserForgotPasswordProcess(
                respUser.Item.StorageKey);
            var respForgot = await ruleService.ExecuteProcessAsync(forgotPasswordProcess);
            Assert.True(respForgot.Success);

            // Verify notification created

            // Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.FORGOT_PASSWORD);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), UserStorageKey.ToString());
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
        }
    }
}