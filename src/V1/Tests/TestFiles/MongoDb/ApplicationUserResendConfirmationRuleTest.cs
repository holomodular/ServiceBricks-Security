using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserResendConfirmationRuleTest
    {
        public ApplicationUserResendConfirmationRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
        }

        public ISystemManager SystemManager { get; set; }

        [Fact]
        public async Task TestRule()
        {
            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            //Execute ApplicationUserRegisterProcess
            string email = Guid.NewGuid().ToString().ToUpper() + SecurityTestConstants.EMAIL_ATSUFFIX;
            UserRegisterProcess registerProcess = new UserRegisterProcess(
                email,
                SecurityTestConstants.PASSWORD);
            var respRegister = await ruleService.ExecuteProcessAsync(registerProcess);
            Assert.True(respRegister.Success);

            //Find User
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByEmailAsync(email);
            Assert.True(respUser.Item != null);

            //Execute ApplicationUserResendConfirmationProcess
            UserResendConfirmationProcess resendEvent = new UserResendConfirmationProcess(
                respUser.Item.StorageKey);
            var respResend = await ruleService.ExecuteProcessAsync(resendEvent);
            Assert.True(respResend.Success);

            //Verify notification email
        }
    }
}