using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserTokenApiControllerTest : UserTokenApiControllerTestBase
    {
        public UserTokenApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserTokenDto>>();
            CreateDependencies();
        }

        public override UserApiControllerTestBase GetAppUserTest()
        {
            return new UserApiControllerTest();
        }
    }
}