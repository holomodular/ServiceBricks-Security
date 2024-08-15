using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserAuditApiControllerTest : UserAuditApiControllerTestBase
    {
        public UserAuditApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserAuditDto>>();
            CreateDependencies();
        }

        public override UserApiControllerTestBase GetAppUserTest()
        {
            return new UserApiControllerTest();
        }
    }
}