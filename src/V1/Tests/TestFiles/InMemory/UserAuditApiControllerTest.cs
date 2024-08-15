using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserAuditApiControllerTest : Integration.UserAuditApiControllerTestBase
    {
        public UserAuditApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserAuditDto>>();
            CreateDependencies();
        }

        public override UserApiControllerTestBase GetAppUserTest()
        {
            return new UserApiControllerTest();
        }
    }
}