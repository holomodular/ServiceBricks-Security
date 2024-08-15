using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserApiControllerTest : UserApiControllerTestBase
    {
        public UserApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserDto>>();
        }
    }
}