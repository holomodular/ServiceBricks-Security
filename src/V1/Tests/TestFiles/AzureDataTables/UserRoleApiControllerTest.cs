using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserRoleApiControllerTest : UserRoleApiControllerTestBase
    {
        public UserRoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserRoleDto>>();
            CreateDependencies();
        }

        public override RoleApiControllerTestBase GetAppRoleTest()
        {
            return new RoleApiControllerTest();
        }

        public override UserApiControllerTestBase GetAppUserTest()
        {
            return new UserApiControllerTest();
        }
    }
}