namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserLogoutRuleTest : UserLogoutRuleTestBase
    {
        public ApplicationUserLogoutRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupPostgres));
        }
    }
}