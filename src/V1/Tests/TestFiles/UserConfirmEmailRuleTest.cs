using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class UserConfirmEmailRuleTestBase : IDisposable, IAsyncDisposable
    {
        public UserConfirmEmailRuleTestBase()
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
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string AuditStorageKey { get; set; }

        [Fact]
        public virtual async Task TestRule()
        {
            var ruleService = SystemManager.ServiceProvider.GetRequiredService<IBusinessRuleService>();

            // Execute ApplicationUserRegisterProcess
            Email = Guid.NewGuid().ToString().ToUpper() + SecurityTestConstants.EMAIL_ATSUFFIX;
            Password = SecurityTestConstants.PASSWORD;
            UserRegisterProcess registerProcess = new UserRegisterProcess(
                Email,
                Password);
            var respRegister = await ruleService.ExecuteProcessAsync(registerProcess);
            if (respRegister.Error)
                throw new Exception(respRegister.ToString());
            Assert.True(respRegister.Success);

            // Find User
            var userManager = SystemManager.ServiceProvider.GetRequiredService<IUserManagerService>();
            var respUser = await userManager.FindByEmailAsync(Email);
            Assert.True(respUser.Item != null);
            var user = respUser.Item;
            UserStorageKey = user.StorageKey;

            // Create confirmation code
            var respCode = await userManager.GenerateEmailConfirmationTokenAsync(user.StorageKey);

            // Execute ApplicationUserConfirmEmailProcess
            UserConfirmEmailProcess confirmEmailProcess = new UserConfirmEmailProcess(
                user.StorageKey, respCode.Item);
            var respConfirmEmail = await ruleService.ExecuteProcessAsync(confirmEmailProcess);
            Assert.True(respConfirmEmail.Success);

            //Verify user email confirmed
            var userService = SystemManager.ServiceProvider.GetRequiredService<IUserApiService>();
            //QueryBuilder queryBuilder = new QueryBuilder();
            //queryBuilder.IsEqual(nameof(UserDto.StorageKey), UserId.ToString());
            //var respUser = await userService.QueryAsync(queryBuilder.Build());
            respUser = await userService.GetAsync(UserStorageKey);
            Assert.True(respUser != null && respUser.Item != null);
            Assert.True(respUser?.Item?.EmailConfirmed == true);

            //Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.CONFIRM_EMAIL_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), UserStorageKey);
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);
            AuditStorageKey = respAudit.Item.List[0].StorageKey;
        }
    }
}