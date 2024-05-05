using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class AuthenticationApiControllerTest : ApplicationUserConfirmEmailRuleTest
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
    }
}