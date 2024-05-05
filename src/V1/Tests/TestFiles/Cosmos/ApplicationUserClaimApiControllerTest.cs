using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserClaimApiControllerTest : ApiControllerTest<ApplicationUserClaimDto>
    {
        public ApplicationUserClaimApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserClaimDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiControllerTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserClaimTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}