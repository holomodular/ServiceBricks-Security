namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserConfirmEmailRuleTest : ApplicationUserConfirmEmailRuleTestBase
    {
        public ApplicationUserConfirmEmailRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlServer));
        }
    }
}