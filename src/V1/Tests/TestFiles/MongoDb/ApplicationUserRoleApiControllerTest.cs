using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRoleApiControllerTest : ApiControllerTest<ApplicationUserRoleDto>
    {
        public ApplicationUserRoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupMongoDb));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserRoleDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiControllerTest();
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);

            var appRoleTest = new ApplicationRoleApiControllerTest();
            var role = appRoleTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole =
                appRoleTest.CreateBase(role);

            role = appRoleTest.TestManager.GetMaximumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole2 =
                appRoleTest.CreateBase(role);
        }
    }
}