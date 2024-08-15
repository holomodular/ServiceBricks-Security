using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationRoleClaimApiControllerTest : Integration.RoleClaimApiControllerTestBase
    {
        public ApplicationRoleClaimApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<RoleClaimDto>>();
            CreateDependencies();
        }

        public override RoleApiControllerTestBase GetAppRoleTest()
        {
            return new RoleApiControllerTest();
        }
    }
}