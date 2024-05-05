using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class ApplicationUserRoleApiControllerTest : ApiControllerTest<ApplicationUserRoleDto>
    {
        public ApplicationUserRoleApiControllerTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = SystemManager.ServiceProvider.GetRequiredService<ITestManager<ApplicationUserRoleDto>>();
            CreateDependencies();
        }

        protected virtual void CreateDependencies()
        {
            var appUserTest = new ApplicationUserApiControllerTest();
            var user = appUserTest.TestManager.GetMinimumDataObject();
            var usercontroller = appUserTest.TestManager.GetController(SystemManager.ServiceProvider);
            var respCreateUser = usercontroller.Create(user);
            ((ApplicationUserRoleTestManager)TestManager).ApplicationUser = ((OkObjectResult)respCreateUser).Value as ApplicationUserDto;

            var appRoleTest = new ApplicationRoleApiControllerTest();
            var role = appRoleTest.TestManager.GetMinimumDataObject();
            var rolecontroller = appRoleTest.TestManager.GetController(SystemManager.ServiceProvider);
            var respCreateRole = rolecontroller.Create(role);
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole = ((OkObjectResult)respCreateRole).Value as ApplicationRoleDto;

            var role2 = appRoleTest.TestManager.GetMaximumDataObject();
            var respCreateRole2 = rolecontroller.Create(role);
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole2 = ((OkObjectResult)respCreateRole2).Value as ApplicationRoleDto;
        }
    }
}