using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Xunit;
using ServiceQuery;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserInvalidPasswordRuleTest //: IDisposable
    {
        public ApplicationUserInvalidPasswordRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
        }

        public ISystemManager SystemManager { get; set; }

        //public virtual void Dispose()
        //{
        //    SystemManager?.StopSystem();
        //}

        [Fact]
        public async Task TestRule()
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
            //Execute ApplicationUserInvalidPasswordProcess
            UserInvalidPasswordProcess invalidProcess = new UserInvalidPasswordProcess(
                user.StorageKey, user.Email);
            var respInvalidPassword = await ruleService.ExecuteProcessAsync(invalidProcess);
            Assert.True(respInvalidPassword.Success);

            // Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(AuditUserDto.AuditName), AuditType.INVALID_PASSWORD);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(AuditUserDto.UserStorageKey), user.StorageKey);
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
        }
    }
}