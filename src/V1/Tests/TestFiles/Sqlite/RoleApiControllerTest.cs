using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class RoleApiControllerTest : RoleApiControllerTestBase
    {
        public RoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<RoleDto>>();
        }
    }
}