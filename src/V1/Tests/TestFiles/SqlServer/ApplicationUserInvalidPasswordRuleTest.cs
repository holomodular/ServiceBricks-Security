namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserInvalidPasswordRuleTest : ApplicationUserInvalidPasswordRuleTestBase
    {
        public ApplicationUserInvalidPasswordRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlServer));
        }
    }
}