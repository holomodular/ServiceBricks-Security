using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.Client.Xunit;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class UserAuditApiClientTest : ApiClientTest<UserAuditDto>
    {
        public UserAuditApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<UserAuditDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiClientTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((UserAuditTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}