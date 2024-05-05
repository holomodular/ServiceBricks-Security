using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBricks.Xunit;
using ServiceBricks.Security;
using ServiceBricks.Security.Client.Xunit;

namespace ServiceBricks.Xunit.Integration.ReturnResponseTests
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationRoleClaimApiClientReturnResponseTest : ApiClientReturnResponseTest<ApplicationRoleClaimDto>
    {
        public ApplicationRoleClaimApiClientReturnResponseTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationRoleClaimDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationRoleApiClientTest();
            appUserTest.SystemManager = this.SystemManager;
            var role = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationRoleClaimTestManager)TestManager).ApplicationRole =
                appUserTest.CreateBase(role);
        }
    }
}