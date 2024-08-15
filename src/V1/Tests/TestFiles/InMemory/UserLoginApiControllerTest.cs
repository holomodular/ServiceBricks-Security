using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserLoginApiControllerTest : UserLoginApiControllerTestBase
    {
        public UserLoginApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserLoginDto>>();
            CreateDependencies();
        }

        public override UserApiControllerTestBase GetAppUserTest()
        {
            return new UserApiControllerTest();
        }
    }
}