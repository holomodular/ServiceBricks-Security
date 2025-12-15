using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class RoleClaimApiControllerTest : RoleClaimApiControllerTestBase
    {
        public RoleClaimApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<RoleClaimDto>>();
            CreateDependencies();
        }

        public override RoleApiControllerTestBase GetAppRoleTest()
        {
            return new RoleApiControllerTest();
        }
    }
}