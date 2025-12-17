using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.Security;
using ServiceBricks.Security.Client.Xunit;
using ServiceQuery;
using System;

namespace ServiceBricks.Xunit.Integration
{
    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class AuthenticationApiClientReturnResponseTest 
    {
        public virtual ISystemManager SystemManager { get; set; }

        public AuthenticationApiClientReturnResponseTest() : base()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(ClientStartup));
        }

        [Fact]
        public virtual async Task AuthenticateReturnResponseAsync()
        {

            var appconfig = SystemManager.ServiceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();

            var apiconfig = config.GetApiConfig();

            var client = new AuthenticationApiClient(
                SystemManager.ServiceProvider.GetRequiredService<IHttpClientFactory>(),
                config);

            // Good request
            AccessTokenRequest request = new AccessTokenRequest();            
            request.client_id = apiconfig.TokenClient;
            request.client_secret = apiconfig.TokenSecret;
            var respAuth = await client.AuthenticateUserAsync(request);
            Assert.True(respAuth.Success && !string.IsNullOrEmpty(respAuth.Item.access_token));

            //// Bad password
            //request = new AccessTokenRequest();
            //request.client_id = apiconfig.TokenClient;
            //request.client_secret = apiconfig.TokenSecret + "z";
            //respAuth = await client.AuthenticateUserAsync(request);
            //Assert.True(respAuth.Error);

        }

        [Fact]
        public virtual void AuthenticateReturnResponse()
        {

            var appconfig = SystemManager.ServiceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();

            var apiconfig = config.GetApiConfig();

            var client = new AuthenticationApiClient(
                SystemManager.ServiceProvider.GetRequiredService<IHttpClientFactory>(),
                config);

            // Good request
            AccessTokenRequest request = new AccessTokenRequest();
            request.client_id = apiconfig.TokenClient;
            request.client_secret = apiconfig.TokenSecret;
            var respAuth = client.AuthenticateUser(request);
            Assert.True(respAuth.Success && !string.IsNullOrEmpty(respAuth.Item.access_token));


            //// Bad password
            //request = new AccessTokenRequest();
            //request.client_id = apiconfig.TokenClient;
            //request.client_secret = apiconfig.TokenSecret + "z";
            //respAuth = client.AuthenticateUser(request);
            //Assert.True(respAuth.Error);

        }


    }
}