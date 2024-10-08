﻿using ServiceBricks.Xunit.Integration;

namespace ServiceBricks.Xunit.Rules
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public partial class ApplicationUserInvalidPasswordRuleTest : UserInvalidPasswordRuleTestBase
    {
        public ApplicationUserInvalidPasswordRuleTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
        }
    }
}