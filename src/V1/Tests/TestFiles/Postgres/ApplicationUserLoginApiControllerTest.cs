using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserLoginApiControllerTest : ApiControllerTest<ApplicationUserLoginDto>
    {
        public ApplicationUserLoginApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupPostgres));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserLoginDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiControllerTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserLoginTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}