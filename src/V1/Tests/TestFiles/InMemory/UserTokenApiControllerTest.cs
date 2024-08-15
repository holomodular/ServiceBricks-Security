using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserTokenApiControllerTest : Integration.UserTokenApiControllerTestBase
    {
        public UserTokenApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserTokenDto>>();
            CreateDependencies();
        }

        public override UserApiControllerTestBase GetAppUserTest()
        {
            return new UserApiControllerTest();
        }
    }
}