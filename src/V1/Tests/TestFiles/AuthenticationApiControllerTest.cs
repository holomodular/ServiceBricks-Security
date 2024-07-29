using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public abstract class AuthenticationApiControllerTestBase : ApplicationUserConfirmEmailRuleTestBase
    {
        [Fact]
        public override async Task TestRule()
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

            // Bad password2
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password + "z";
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass2)
            {
                Assert.True(badResultPass2.Value != null);
                if (badResultPass2.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Bad password3
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password + "z";
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass3)
            {
                Assert.True(badResultPass3.Value != null);
                if (badResultPass3.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Bad password4
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password + "z";
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass4)
            {
                Assert.True(badResultPass4.Value != null);
                if (badResultPass4.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Bad password5
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password + "z";
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass5)
            {
                Assert.True(badResultPass5.Value != null);
                if (badResultPass5.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

            // Bad password6
            request = new AccessTokenRequest();
            request.client_id = this.Email;
            request.client_secret = this.Password + "z";
            respAuth = await authenticationApiController.AuthenticateUserAsync(request);
            if (respAuth is BadRequestObjectResult badResultPass6)
            {
                Assert.True(badResultPass6.Value != null);
                if (badResultPass6.Value is ProblemDetails problemDetails)
                    Assert.True(problemDetails.Title == LocalizationResource.ERROR_SYSTEM);
                else
                    Assert.Fail("");
            }
            else
                Assert.Fail("");

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