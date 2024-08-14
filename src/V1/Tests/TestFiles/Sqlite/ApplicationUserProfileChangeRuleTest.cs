﻿namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserProfileChangeRuleTest : ApplicationUserProfileChangeRuleTestBase
    {
        public ApplicationUserProfileChangeRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupSqlite));
        }
    }
}