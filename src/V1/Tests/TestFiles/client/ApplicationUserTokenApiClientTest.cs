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

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserTokenApiClientTest : ApiClientTest<ApplicationUserTokenDto>
    {
        public ApplicationUserTokenApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserTokenDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiClientTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserTokenTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}