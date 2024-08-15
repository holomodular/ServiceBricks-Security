namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserResendConfirmationRuleTest : UserResendConfirmationRuleTestBase
    {
        public ApplicationUserResendConfirmationRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlServer));
        }
    }
}