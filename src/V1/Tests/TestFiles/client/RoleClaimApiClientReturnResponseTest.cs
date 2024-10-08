﻿using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.Client.Xunit;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationRoleClaimApiClientReturnResponseTest : ApiClientReturnResponseTest<RoleClaimDto>
    {
        public ApplicationRoleClaimApiClientReturnResponseTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<RoleClaimDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationRoleApiClientTest();
            appUserTest.SystemManager = this.SystemManager;
            var role = appUserTest.TestManager.GetMinimumDataObject();
            ((RoleClaimTestManager)TestManager).ApplicationRole =
                appUserTest.CreateBase(role);
        }
    }
}