using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    public class UserLoginStubTestManager : UserLoginTestManager
    {
        public class UserLoginHttpClientFactory : IHttpClientFactory
        {
            private ApiClientTests.CustomGenericHttpClientHandler<UserLoginDto> _handler;

            public UserLoginHttpClientFactory(ApiClientTests.CustomGenericHttpClientHandler<UserLoginDto> handler)
            {
                _handler = handler;
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        public override IApiClient<UserLoginDto> GetClient(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IUserLoginApiService>();
            var controller = new UserLoginApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserLoginDto>(controller);
            var clientHandlerFactory = new UserLoginHttpClientFactory(handler);
            return new UserLoginApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<UserLoginDto> Handler { get; set; }

        public override IApiClient<UserLoginDto> GetClientReturnResponse(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IUserLoginApiService>();
            var controller = new UserLoginApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserLoginDto>(controller);
            var clientHandlerFactory = new UserLoginHttpClientFactory(handler);
            return new UserLoginApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserLoginApiClientTest : ApiClientTest<UserLoginDto>
    {
        public StubUserLoginApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserLoginStubTestManager();
            ((UserLoginStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserLoginApiClientReturnResponseTests : ApiClientReturnResponseTest<UserLoginDto>
    {
        public StubUserLoginApiClientReturnResponseTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserLoginStubTestManager();
            ((UserLoginStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }
}