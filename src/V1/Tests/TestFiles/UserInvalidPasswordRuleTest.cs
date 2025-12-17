using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract partial class UserInvalidPasswordRuleTestBase : IDisposable, IAsyncDisposable
    {
        public UserInvalidPasswordRuleTestBase()
        {
        }

        public ISystemManager SystemManager { get; set; }


        public void Dispose()
        {
            CleanupDependencies();
        }

        public async ValueTask DisposeAsync()
        {
            await CleanupDependenciesAsync();
        }

        protected virtual void CleanupDependencies()
        {
            string userstoragekey = UserStorageKey;

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
        }

        protected virtual async Task CleanupDependenciesAsync()
        {
            string userstoragekey = UserStorageKey;

            // Cleanup Audits
            var audituserservice = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var auditquery = new ServiceQueryRequestBuilder().IsEqual(nameof(UserAuditDto.UserStorageKey), userstoragekey).Build();
            var respAudits = await audituserservice.QueryAsync(auditquery);
            foreach (var item in respAudits.Item.List)
                await audituserservice.DeleteAsync(item.StorageKey);

            // Cleanup UserRoles
            var userroleservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserRoleDto>>();
            var roleq = new ServiceQueryRequestBuilder().IsEqual(nameof(UserRoleDto.UserStorageKey), userstoragekey).Build();
            var respUserRoles = await userroleservice.QueryAsync(roleq);
            foreach (var item in respUserRoles.Item.List)
                await userroleservice.DeleteAsync(item.StorageKey);

            // Cleanup User
            var userservice = SystemManager.ServiceProvider.GetRequiredService<IApiService<UserDto>>();
            await userservice.DeleteAsync(userstoragekey);
        }

        public virtual string UserStorageKey { get; set; }

        public virtual string AuditStorageKey { get; set; }

        [Fact]
        public async Task TestRule()
        {
            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Execute ApplicationUserRegisterEvent
            string email = Guid.NewGuid().ToString().ToUpper() + SecurityTestConstants.EMAIL_ATSUFFIX;
            UserRegisterProcess registerProcess = new UserRegisterProcess(
                email,
                SecurityTestConstants.PASSWORD);
            var respRegister = await ruleService.ExecuteProcessAsync(registerProcess);

            var reg = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleRegistry>();
            var list = reg.GetRegistryList(typeof(UserRegisterProcess));
            if (respRegister.Error)
            {
                string er = list.Count.ToString() + " " + respRegister.ToString();
                throw new Exception(er);
            }
            Assert.True(respRegister.Success);

            // Find User
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByEmailAsync(email);
            Assert.True(respUser.Item != null);
            var user = respUser.Item;

            //Execute ApplicationUserInvalidPasswordProcess
            UserInvalidPasswordProcess invalidProcess = new UserInvalidPasswordProcess(
                user.StorageKey, user.Email);
            var respInvalidPassword = await ruleService.ExecuteProcessAsync(invalidProcess);
            Assert.True(respInvalidPassword.Success);
            UserStorageKey = user.StorageKey;

            // Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.INVALID_PASSWORD_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), user.StorageKey);
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            AuditStorageKey = respAudit.Item.List[0].StorageKey;
        }
    }
}