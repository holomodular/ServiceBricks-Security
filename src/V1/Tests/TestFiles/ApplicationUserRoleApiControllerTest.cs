using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class ApplicationUserRoleApiControllerTestBase : ApiControllerTest<ApplicationUserRoleDto>, IDisposable
    {
        public ApplicationUserRoleApiControllerTestBase() : base()
        {
        }

        public virtual void Dispose()
        {
            CleanupDependencies();
        }

        protected virtual void CleanupDependencies()
        {
            string userstoragekey = ((ApplicationUserRoleTestManager)TestManager).ApplicationUser.StorageKey;

            // Cleanup Audits
            var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IAuditUserApiService>();
            var auditquery = new ServiceQueryRequestBuilder().IsEqual(nameof(AuditUserDto.UserStorageKey), userstoragekey).Build();
            var respAudits = audituserservice.Query(auditquery);
            foreach (var item in respAudits.Item.List)
                audituserservice.Delete(item.StorageKey);

            // Cleanup UserRoles
            var userroleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationUserRoleDto>>();
            var roleq = new ServiceQueryRequestBuilder().IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), userstoragekey).Build();
            var respUserRoles = userroleservice.Query(roleq);
            foreach (var item in respUserRoles.Item.List)
                userroleservice.Delete(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationUserDto>>();
            userservice.Delete(userstoragekey);

            string rolestoragekey1 = ((ApplicationUserRoleTestManager)TestManager).ApplicationRole.StorageKey;
            string rolestoragekey2 = ((ApplicationUserRoleTestManager)TestManager).ApplicationRole2.StorageKey;

            // Cleanup Roles
            var roleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationRoleDto>>();
            roleservice.Delete(rolestoragekey1);
            roleservice.Delete(rolestoragekey2);
        }

        public abstract ApplicationUserApiControllerTestBase GetAppUserTest();

        public abstract ApplicationRoleApiControllerTestBase GetAppRoleTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppUserTest();
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);

            var appRoleTest = GetAppRoleTest();
            var role = appRoleTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole =
                appRoleTest.CreateBase(role);

            role = appRoleTest.TestManager.GetMaximumDataObject();
            ((ApplicationUserRoleTestManager)TestManager).ApplicationRole2 =
                appRoleTest.CreateBase(role);
        }
    }
}