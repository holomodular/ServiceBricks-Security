namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserProfileChangeRuleTest : Integration.UserProfileChangeRuleTestBase
    {
        public ApplicationUserProfileChangeRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }
    }
}