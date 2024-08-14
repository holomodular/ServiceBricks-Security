using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class AuditUserApiControllerTest : AuditUserApiControllerTestBase
    {
        public AuditUserApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<AuditUserDto>>();
            CreateDependencies();
        }

        public override ApplicationUserApiControllerTestBase GetAppUserTest()
        {
            return new ApplicationUserApiControllerTest();
        }
    }
}