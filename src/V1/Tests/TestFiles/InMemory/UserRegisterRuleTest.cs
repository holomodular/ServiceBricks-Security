namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRegisterRuleTest : Integration.UserRegisterRuleTestBase
    {
        public ApplicationUserRegisterRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }
    }
}