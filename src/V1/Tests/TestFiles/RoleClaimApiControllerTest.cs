using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class RoleClaimApiControllerTestBase : ApiControllerTest<RoleClaimDto>, IDisposable
    {
        public RoleClaimApiControllerTestBase() : base()
        {
        }

        public void Dispose()
        {
            CleanupDependencies();
        }

        public abstract RoleApiControllerTestBase GetAppRoleTest();

        protected virtual void CreateDependencies()
        {
            var appUserTest = GetAppRoleTest();
            appUserTest.SystemManager = this.SystemManager;
            var role = appUserTest.TestManager.GetMinimumDataObject();
            ((RoleClaimTestManager)TestManager).ApplicationRole =
                appUserTest.CreateBase(role);
        }

        protected virtual void CleanupDependencies()
        {
            string rolestoragekey = ((RoleClaimTestManager)TestManager).ApplicationRole.StorageKey;

            // Cleanup RoleClaims
            var roleclaimservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<RoleClaimDto>>();
            var claimq = new ServiceQueryRequestBuilder().IsEqual(nameof(RoleClaimDto.RoleStorageKey), rolestoragekey).Build();
            var respRoleClaims = roleclaimservice.Query(claimq);
            foreach (var item in respRoleClaims.Item.List)
                roleclaimservice.Delete(item.StorageKey);

            // Cleanup Role
            var roleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<RoleDto>>();
            roleservice.Delete(rolestoragekey);
        }
    }
}