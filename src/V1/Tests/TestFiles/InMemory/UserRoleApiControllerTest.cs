using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserRoleApiControllerTest : Integration.UserRoleApiControllerTestBase
    {
        public UserRoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
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