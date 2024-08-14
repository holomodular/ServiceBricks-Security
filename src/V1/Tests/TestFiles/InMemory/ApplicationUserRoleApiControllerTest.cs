using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRoleApiControllerTest : Integration.ApplicationUserRoleApiControllerTestBase
    {
        public ApplicationUserRoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserRoleDto>>();
            CreateDependencies();
        }

        public override ApplicationRoleApiControllerTestBase GetAppRoleTest()
        {
            return new ApplicationRoleApiControllerTest();
        }

        public override ApplicationUserApiControllerTestBase GetAppUserTest()
        {
            return new ApplicationUserApiControllerTest();
        }
    }
}