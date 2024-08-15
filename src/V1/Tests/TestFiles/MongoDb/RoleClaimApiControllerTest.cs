using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationRoleClaimApiControllerTest : RoleClaimApiControllerTestBase
    {
        public ApplicationRoleClaimApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<RoleClaimDto>>();
            CreateDependencies();
        }

        public override RoleApiControllerTestBase GetAppRoleTest()
        {
            return new RoleApiControllerTest();
        }
    }
}