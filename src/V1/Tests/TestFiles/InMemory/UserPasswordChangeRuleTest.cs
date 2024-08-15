namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserPasswordChangeRuleTest : Integration.UserPasswordChangeRuleTestBase
    {
        public ApplicationUserPasswordChangeRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }
    }
}