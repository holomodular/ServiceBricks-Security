using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserConfirmEmailRuleTest : UserConfirmEmailRuleTestBase
    {
        public ApplicationUserConfirmEmailRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }
    }
}