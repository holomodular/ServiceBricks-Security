using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    public class UserRoleStubTestManager : UserRoleTestManager
    {
        public class UserRoleHttpClientFactory : IHttpClientFactory
        {
            private ApiClientTests.CustomGenericHttpClientHandler<UserRoleDto> _handler;

            public UserRoleHttpClientFactory(ApiClientTests.CustomGenericHttpClientHandler<UserRoleDto> handler)
            {
                _handler = handler;
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        public override IApiClient<UserRoleDto> GetClient(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IUserRoleApiService>();
            var controller = new UserRoleApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserRoleDto>(controller);
            var clientHandlerFactory = new UserRoleHttpClientFactory(handler);
            return new UserRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<UserRoleDto> Handler { get; set; }

        public override IApiClient<UserRoleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IUserRoleApiService>();
            var controller = new UserRoleApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<UserRoleDto>(controller);
            var clientHandlerFactory = new UserRoleHttpClientFactory(handler);
            return new UserRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserRoleApiClientTest : ApiClientTest<UserRoleDto>
    {
        public StubUserRoleApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserRoleStubTestManager();
            ((UserRoleStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
            ((UserRoleStubTestManager)TestManager).ApplicationRole = new RoleDto() { StorageKey = Guid.NewGuid().ToString() };
            ((UserRoleStubTestManager)TestManager).ApplicationRole2 = new RoleDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubUserRoleApiClientReturnResponseTests : ApiClientReturnResponseTest<UserRoleDto>
    {
        public StubUserRoleApiClientReturnResponseTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new UserRoleStubTestManager();
            ((UserRoleStubTestManager)TestManager).ApplicationUser = new UserDto() { StorageKey = Guid.NewGuid().ToString() };
            ((UserRoleStubTestManager)TestManager).ApplicationRole = new RoleDto() { StorageKey = Guid.NewGuid().ToString() };
            ((UserRoleStubTestManager)TestManager).ApplicationRole2 = new RoleDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }
}