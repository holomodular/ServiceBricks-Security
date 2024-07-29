﻿using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRoleApiControllerTest : ApplicationUserRoleApiControllerTestBase
    {
        public ApplicationUserRoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupCosmos));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserRoleDto>>();
            CreateDependencies();
        }

        public override ApplicationRoleApiControllerTestBase GetAppRoleTest()
        {
            return new ApplicationRoleApiControllerTest();
        }

        public override ApplicationUserApiControllerTestBase GetAppUserTest()
        {
            return new ApplicationUserApiControllerTest();
        }
    }
}