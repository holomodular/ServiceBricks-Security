using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;

namespace ServiceBricks.Xunit
{
    public class RoleClaimStubTestManager : RoleClaimTestManager
    {        

        public class RoleClaimHttpClientFactory : IHttpClientFactory
        {
            private ApiClientTests.CustomGenericHttpClientHandler<RoleClaimDto> _handler;

            public RoleClaimHttpClientFactory(ApiClientTests.CustomGenericHttpClientHandler<RoleClaimDto> handler)
            {
                _handler = handler;                
            }

            public HttpClient CreateClient(string name)
            {
                return new HttpClient(_handler);
            }
        }

        public override IApiClient<RoleClaimDto> GetClient(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IRoleClaimApiService>();
            var controller = new RoleClaimApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<RoleClaimDto>(controller);
            var clientHandlerFactory = new RoleClaimHttpClientFactory(handler);
            return new RoleClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }

        public ApiClientTests.CustomGenericHttpClientHandler<RoleClaimDto> Handler { get; set; }

        public override IApiClient<RoleClaimDto> GetClientReturnResponse(IServiceProvider serviceProvider)
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
            var apiservice = serviceProvider.GetRequiredService<IRoleClaimApiService>();
            var controller = new RoleClaimApiController(apiservice, apioptions);
            var handler = new ApiClientTests.CustomGenericHttpClientHandler<RoleClaimDto>(controller);
            var clientHandlerFactory = new RoleClaimHttpClientFactory(handler);
            return new RoleClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                clientHandlerFactory,
                config);
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubRoleClaimApiClientTest : ApiClientTest<RoleClaimDto>
    {
        public StubRoleClaimApiClientTest()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new RoleClaimStubTestManager();
            ((RoleClaimStubTestManager)TestManager).ApplicationRole = new RoleDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }

    [Collection(ServiceBricks.Xunit.Constants.SERVICEBRICKS_COLLECTION_NAME)]
    public class StubRoleClaimApiClientReturnResponseTests : ApiClientReturnResponseTest<RoleClaimDto>
    {
        public StubRoleClaimApiClientReturnResponseTests()
        {
            SystemManager = ServiceBricksSystemManager.GetSystemManager(typeof(StartupInMemory));
            TestManager = new RoleClaimStubTestManager();
            ((RoleClaimStubTestManager)TestManager).ApplicationRole = new RoleDto() { StorageKey = Guid.NewGuid().ToString() };
        }
    }
}