using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    public class UserTokenStubTestManager : UserTokenTestManager
    {
        public class UserTokenHttpClientFactory : IHttpClientFactory
        {
            private ApiClientTests.CustomGenericHttpClientHandler<UserTokenDto> _handler;

            public UserTokenHttpClientFactory(ApiClientTests.CustomGenericHttpClientHandler<UserTokenDto> handler)
            {
                _handler = handler;
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        public override IApiClient<UserTokenDto> GetClient(IServiceProvider serviceProvider)
        {
            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":DisableAuthentication", "false" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":TokenUrl", "https://localhost:7000/token" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":BaseServiceUrl", "https://localhost:7000/" },
            })
            .Build();

            var apioptions = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false });
            var apiservice = serviceProvider.GetRequiredService<IUserTokenApiService>();
            var controller = new UserTokenApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserTokenDto>(controller);
            var clientHandlerFactory = new UserTokenHttpClientFactory(handler);
            return new UserTokenApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<UserTokenDto> Handler { get; set; }

        public override IApiClient<UserTokenDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":DisableAuthentication", "false" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":TokenUrl", "https://localhost:7000/token" },
                            { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":BaseServiceUrl", "https://localhost:7000/" },
            })
            .Build();

            var apioptions = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true });
            var apiservice = serviceProvider.GetRequiredService<IUserTokenApiService>();
            var controller = new UserTokenApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserTokenDto>(controller);
            var clientHandlerFactory = new UserTokenHttpClientFactory(handler);
            return new UserTokenApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserTokenApiClientTest : ApiClientTest<UserTokenDto>
    {
        public StubUserTokenApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserTokenStubTestManager();
            ((UserTokenStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserTokenApiClientReturnResponseTests : ApiClientReturnResponseTest<UserTokenDto>
    {
        public StubUserTokenApiClientReturnResponseTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserTokenStubTestManager();
            ((UserTokenStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }
}