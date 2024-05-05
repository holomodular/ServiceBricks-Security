using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationRoleClaimApiControllerTest : ApiControllerTest<ApplicationRoleClaimDto>
    {
        public ApplicationRoleClaimApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationRoleClaimDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationRoleApiControllerTest();
            appUserTest.SystemManager = this.SystemManager;
            var role = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationRoleClaimTestManager)TestManager).ApplicationRole =
                appUserTest.CreateBase(role);
        }
    }
}