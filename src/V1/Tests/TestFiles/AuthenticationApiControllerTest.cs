using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class AuthenticationApiControllerTestBase : UserConfirmEmailRuleTestBase
    {
        [Fact]
        public override async Task TestRule()
        {
            await base.TestRule();

            var authenticationApiController = SystemManager.ServiceProvider.GetRequiredService<IAuthenticationApiController>();

            // Good request
            AccessTokenRequest request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password;
            var respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is OkObjectResult okResultGetItem)
            {
                Assert.True(okResultGetItem.Value != null);
                if (okResultGetItem.Value is AccessTokenResponse atresp)
                {
                    Assert.True(!string.IsNullOrEmpty(atresp.access_token));
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Bad password
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password + "z";
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass)
            {
                Assert.True(badResultPass.Value != null);
                if (badResultPass.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            //Verify audituser created
            var auditUserService = SystemManager.ServiceProvider.GetRequiredService<IUserAuditApiService>();
            var queryBuilder = new ServiceQueryRequestBuilder();
            queryBuilder.IsEqual(nameof(UserAuditDto.AuditType), AuditType.INVALID_PASSWORD_TEXT);
            queryBuilder.And();
            queryBuilder.IsEqual(nameof(UserAuditDto.UserStorageKey), UserStorageKey);
            var respAudit = await auditUserService.QueryAsync(queryBuilder.Build());
            Assert.True(respAudit != null && respAudit.Item.List.Count > 0);

            // Bad email
            request = new AccessTokenRequest();
            request.client_id = "z" + this.Email;
            request.client_secret = this.Password;
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultEmail)
            {
                Assert.True(badResultEmail.Value != null);
                if (badResultEmail.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }

        [Fact]
        public async Task TestLockoutRule()
        {
            await base.TestRule();

            var authenticationApiController = SystemManager.ServiceProvider.GetRequiredService<IAuthenticationApiController>();

            AccessTokenRequest request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password;
            var respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is OkObjectResult okResultGetItem)
            {
                Assert.True(okResultGetItem.Value != null);
                if (okResultGetItem.Value is AccessTokenResponse atresp)
                {
                    Assert.True(!string.IsNullOrEmpty(atresp.access_token));
                }
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Do 5 Bad password attempts
            int count = 1;
            while (count <= 5)
            {
                // Bad password
                request = new AccessTokenRequest();
                request.client_id = this.Email;
                request.client_secret = this.Password + "z";
                respAuth = await authenticationApiController.AuthenticateUserAsync(request);
                if (respAuth is BadRequestObjectResult badResultPass)
                {
                    Assert.True(badResultPass.Value != null);
                    if (badResultPass.Value is ProblemDetails problemDetails)
                        Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                    else
                        Assert.Fail("");
                }
                else
                    Assert.Fail("");

                // increment count
                count++;
            }

            // Use a good password, it will fail now
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password;
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass7)
            {
                Assert.True(badResultPass7.Value != null);
                if (badResultPass7.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");
        }
    }
}