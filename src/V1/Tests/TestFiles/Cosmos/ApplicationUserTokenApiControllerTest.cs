﻿using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserTokenApiControllerTest : ApplicationUserTokenApiControllerTestBase
    {
        public ApplicationUserTokenApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserTokenDto>>();
            CreateDependencies();
        }

        public override ApplicationUserApiControllerTestBase GetAppUserTest()
        {
            return new ApplicationUserApiControllerTest();
        }
    }
}