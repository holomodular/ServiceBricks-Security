namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserPasswordResetRuleTest : UserPasswordResetRuleTestBase
    {
        public ApplicationUserPasswordResetRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
        }
    }
}