using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserTokenApiControllerTest : ApiControllerTest<ApplicationUserTokenDto>
    {
        public ApplicationUserTokenApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserTokenDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiControllerTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserTokenTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}