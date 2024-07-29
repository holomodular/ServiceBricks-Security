using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class ApplicationUserClaimApiControllerTestBase : ApiControllerTest<ApplicationUserClaimDto>, IDisposable
    {
        public ApplicationUserClaimApiControllerTestBase() : base()
        {
        }

        public void Dispose()
        {
            CleanupDependencies();
        }

        public abstract ApplicationUserApiControllerTestBase GetAppUserTest();

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

            // Cleanup UserClaims
            var userclaimservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationUserClaimDto>>();
            var claimq = new ServiceQueryRequestBuilder().IsEqual(nameof(ApplicationUserClaimDto.UserStorageKey), userstoragekey).Build();
            var respUserClaims = userclaimservice.Query(claimq);
            foreach (var item in respUserClaims.Item.List)
                userclaimservice.Delete(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationUserDto>>();
            userservice.Delete(userstoragekey);
        }
    }
}