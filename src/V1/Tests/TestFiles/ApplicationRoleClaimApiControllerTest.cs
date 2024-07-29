using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class ApplicationRoleClaimApiControllerTestBase : ApiControllerTest<ApplicationRoleClaimDto>, IDisposable
    {
        public ApplicationRoleClaimApiControllerTestBase() : base()
        {
        }

        public void Dispose()
        {
            CleanupDependencies();
        }

        public abstract ApplicationRoleApiControllerTestBase GetAppRoleTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppRoleTest();
            appUserTest.SystemManager = this.SystemManager;
            var role = appUserTest.TestManager.GetMinimumDataObject();
            ((ApplicationRoleClaimTestManager)TestManager).ApplicationRole =
                appUserTest.CreateBase(role);
        }

        protected virtual void CleanupDependencies()
        {
            string rolestoragekey = ((ApplicationRoleClaimTestManager)TestManager).ApplicationRole.StorageKey;

            // Cleanup RoleClaims
            var roleclaimservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationRoleClaimDto>>();
            var claimq = new ServiceQueryRequestBuilder().IsEqual(nameof(ApplicationRoleClaimDto.RoleStorageKey), rolestoragekey).Build();
            var respRoleClaims = roleclaimservice.Query(claimq);
            foreach (var item in respRoleClaims.Item.List)
                roleclaimservice.Delete(item.StorageKey);

            // Cleanup Role
            var roleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<ApplicationRoleDto>>();
            roleservice.Delete(rolestoragekey);
        }
    }
}