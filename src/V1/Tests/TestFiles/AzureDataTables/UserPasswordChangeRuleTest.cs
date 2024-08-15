namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserPasswordChangeRuleTest : UserPasswordChangeRuleTestBase
    {
        public ApplicationUserPasswordChangeRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupAzureDataTables));
        }
    }
}