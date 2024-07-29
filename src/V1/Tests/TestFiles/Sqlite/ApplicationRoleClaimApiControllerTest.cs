using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationRoleClaimApiControllerTest : ApplicationRoleClaimApiControllerTestBase
    {
        public ApplicationRoleClaimApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationRoleClaimDto>>();
            CreateDependencies();
        }

        public override ApplicationRoleApiControllerTestBase GetAppRoleTest()
        {
            return new ApplicationRoleApiControllerTest();
        }
    }
}