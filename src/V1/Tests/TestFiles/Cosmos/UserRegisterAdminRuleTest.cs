namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRegisterAdminRuleTest : UserRegisterAdminRuleTestBase
    {
        public ApplicationUserRegisterAdminRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
        }
    }
}