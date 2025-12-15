using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    public class UserClaimStubTestManager : UserClaimTestManager
    {
        public class UserClaimHttpClientFactory : IHttpClientFactory
        {
            private ApiClientTests.CustomGenericHttpClientHandler<UserClaimDto> _handler;

            public UserClaimHttpClientFactory(ApiClientTests.CustomGenericHttpClientHandler<UserClaimDto> handler)
            {
                _handler = handler;
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        public override IApiClient<UserClaimDto> GetClient(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IUserClaimApiService>();
            var controller = new UserClaimApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserClaimDto>(controller);
            var clientHandlerFactory = new UserClaimHttpClientFactory(handler);
            return new UserClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<UserClaimDto> Handler { get; set; }

        public override IApiClient<UserClaimDto> GetClientReturnResponse(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IUserClaimApiService>();
            var controller = new UserClaimApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserClaimDto>(controller);
            var clientHandlerFactory = new UserClaimHttpClientFactory(handler);
            return new UserClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserClaimApiClientTest : ApiClientTest<UserClaimDto>
    {
        public StubUserClaimApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserClaimStubTestManager();
            ((UserClaimStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserClaimApiClientReturnResponseTests : ApiClientReturnResponseTest<UserClaimDto>
    {
        public StubUserClaimApiClientReturnResponseTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserClaimStubTestManager();
            ((UserClaimStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }
}