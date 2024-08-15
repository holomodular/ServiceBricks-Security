using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.Client.Xunit;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRoleApiClientReturnResponseTest : ApiClientReturnResponseTest<UserRoleDto>
    {
        public ApplicationUserRoleApiClientReturnResponseTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserRoleDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiClientTest();
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((UserRoleTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);

            var appRoleTest = new ApplicationRoleApiClientTest();
            var role = appRoleTest.TestManager.GetMinimumDataObject();
            ((UserRoleTestManager)TestManager).ApplicationRole =
                appRoleTest.CreateBase(role);

            var role2 = appRoleTest.TestManager.GetMaximumDataObject();
            ((UserRoleTestManager)TestManager).ApplicationRole2 =
                appRoleTest.CreateBase(role);
        }
    }
}