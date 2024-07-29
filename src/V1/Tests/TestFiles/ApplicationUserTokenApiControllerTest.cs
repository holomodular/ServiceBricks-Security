using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class ApplicationUserTokenApiControllerTestBase : ApiControllerTest<ApplicationUserTokenDto>, IDisposable
    {
        public ApplicationUserTokenApiControllerTestBase() : base()
        {
        }

        public virtual void Dispose()
        {
            CleanupDependencies();
        }

        protected virtual void CleanupDependencies()
        {
            string userstoragekey = ((ApplicationUserTokenTestManager)TestManager).ApplicationUser.StorageKey;

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

            // Cleanup UserTokens
            var usertokenservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationUserTokenDto>>();
            var tokenq = new ServiceQueryRequestBuilder().IsEqual(nameof(ApplicationUserTokenDto.UserStorageKey), userstoragekey).Build();
            var respUserTokens = usertokenservice.Query(tokenq);
            foreach (var item in respUserTokens.Item.List)
                usertokenservice.Delete(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationUserDto>>();
            userservice.Delete(userstoragekey);
        }

        public abstract ApplicationUserApiControllerTestBase GetAppUserTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppUserTest();
            appUserTest.SystemManager = this.SystemManager;
            var user = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationUserTokenTestManager)TestManager).ApplicationUser =
                appUserTest.CreateBase(user);
        }
    }
}