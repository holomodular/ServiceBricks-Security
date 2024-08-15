namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserForgotPasswordRuleTest : UserForgotPasswordRuleTestBase
    {
        public ApplicationUserForgotPasswordRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
        }
    }
}