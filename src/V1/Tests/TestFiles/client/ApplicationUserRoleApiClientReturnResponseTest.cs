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
    public class ApplicationUserRoleApiClientReturnResponseTest : ApiClientReturnResponseTest<ApplicationUserRoleDto>
    {
        public ApplicationUserRoleApiClientReturnResponseTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserRoleDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiClientTest();
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);

            var appRoleTest = new ApplicationRoleApiClientTest();
            var role = appRoleTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole =
                appRoleTest.CreateBase(role);

            var role2 = appRoleTest.TestManager.GetMaximumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole2 =
                appRoleTest.CreateBase(role);
        }
    }
}