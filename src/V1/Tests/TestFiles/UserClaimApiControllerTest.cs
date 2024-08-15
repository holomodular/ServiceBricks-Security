using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class UserClaimApiControllerTestBase : ApiControllerTest<UserClaimDto>, IDisposable
    {
        public UserClaimApiControllerTestBase() : base()
        {
        }

        public void Dispose()
        {
            CleanupDependencies();
        }

        public abstract UserApiControllerTestBase GetAppUserTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppUserTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserClaimTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }

        protected virtual void CleanupDependencies()
        {
            string userstoragekey = ((ApplicationUserClaimTestManager)TestManager).ApplicationUser.StorageKey;

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

            // Cleanup UserClaims
            var userclaimservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserClaimDto>>();
            var claimq = new ServiceQueryRequestBuilder().IsEqual(nameof(UserClaimDto.UserStorageKey), userstoragekey).Build();
            var respUserClaims = userclaimservice.Query(claimq);
            foreach (var item in respUserClaims.Item.List)
                userclaimservice.Delete(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserDto>>();
            userservice.Delete(userstoragekey);
        }
    }
}