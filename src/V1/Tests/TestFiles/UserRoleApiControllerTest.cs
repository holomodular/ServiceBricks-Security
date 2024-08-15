using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class UserRoleApiControllerTestBase : ApiControllerTest<UserRoleDto>, IDisposable
    {
        public UserRoleApiControllerTestBase() : base()
        {
        }

        public virtual void Dispose()
        {
            CleanupDependencies();
        }

        protected virtual void CleanupDependencies()
        {
            string userstoragekey = ((UserRoleTestManager)TestManager).ApplicationUser.StorageKey;

            // Cleanup Audits
            var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var auditquery = new ServiceQueryRequestBuilder().IsEqual(nameof(UserAuditDto.UserStorageKey), userstoragekey).Build();
            var respAudits = audituserservice.Query(auditquery);
            foreach (var item in respAudits.Item.List)
                audituserservice.Delete(item.StorageKey);

            // Cleanup UserRoles
            var userroleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserRoleDto>>();
            var roleq = new ServiceQueryRequestBuilder().IsEqual(nameof(UserRoleDto.UserStorageKey), userstoragekey).Build();
            var respUserRoles = userroleservice.Query(roleq);
            foreach (var item in respUserRoles.Item.List)
                userroleservice.Delete(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserDto>>();
            userservice.Delete(userstoragekey);

            string rolestoragekey1 = ((UserRoleTestManager)TestManager).ApplicationRole.StorageKey;
            string rolestoragekey2 = ((UserRoleTestManager)TestManager).ApplicationRole2.StorageKey;

            // Cleanup Roles
            var roleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<RoleDto>>();
            roleservice.Delete(rolestoragekey1);
            roleservice.Delete(rolestoragekey2);
        }

        public abstract UserApiControllerTestBase GetAppUserTest();

        public abstract RoleApiControllerTestBase GetAppRoleTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppUserTest();
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((UserRoleTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);

            var appRoleTest = GetAppRoleTest();
            var role = appRoleTest.TestManager.GetMinimumDataObject();
            ((UserRoleTestManager)TestManager).ApplicationRole =
                appRoleTest.CreateBase(role);

            role = appRoleTest.TestManager.GetMaximumDataObject();
            ((UserRoleTestManager)TestManager).ApplicationRole2 =
                appRoleTest.CreateBase(role);
        }
    }
}