﻿namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class AuthenticationApiControllerTest : AuthenticationApiControllerTestBase
    {
        public AuthenticationApiControllerTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupPostgres));
        }
    }
}