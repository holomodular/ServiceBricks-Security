using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class UserTokenApiControllerTestBase : ApiControllerTest<UserTokenDto>, IDisposable
    {
        public UserTokenApiControllerTestBase() : base()
        {
        }

        public virtual void Dispose()
        {
            CleanupDependencies();
        }

        protected virtual void CleanupDependencies()
        {
            string userstoragekey = ((UserTokenTestManager)TestManager).ApplicationUser.StorageKey;

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

            // Cleanup UserTokens
            var usertokenservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserTokenDto>>();
            var tokenq = new ServiceQueryRequestBuilder().IsEqual(nameof(UserTokenDto.UserStorageKey), userstoragekey).Build();
            var respUserTokens = usertokenservice.Query(tokenq);
            foreach (var item in respUserTokens.Item.List)
                usertokenservice.Delete(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserDto>>();
            userservice.Delete(userstoragekey);
        }

        public abstract UserApiControllerTestBase GetAppUserTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppUserTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((UserTokenTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}