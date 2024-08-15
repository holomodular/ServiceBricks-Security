using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserApiControllerTest : Integration.UserApiControllerTestBase
    {
        public UserApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserDto>>();
        }
    }
}