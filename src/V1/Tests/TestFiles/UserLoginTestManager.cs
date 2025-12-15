using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbUserLoginTestManager : UserLoginTestManager
    {
        public override UserLoginDto GetObjectNotFound()
        {
            return new UserLoginDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class UserLoginTestManager : TestManager<UserLoginDto>
    {
        public virtual UserDto ApplicationUser { get; set; }

        public override UserLoginDto GetMinimumDataObject()
        {
            return new UserLoginDto()
            {
                LoginProvider = Guid.NewGuid().ToString(),
                ProviderKey = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey,
            };
        }

        public override UserLoginDto GetMaximumDataObject()
        {
            var model = new UserLoginDto()
            {
                LoginProvider = Guid.NewGuid().ToString(),
                ProviderDisplayName = Guid.NewGuid().ToString(),
                ProviderKey = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey,
            };
            return model;
        }

        public override IApiController<UserLoginDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new UserLoginApiController(serviceProvider.GetRequiredService<IUserLoginApiService>(), options);
        }

        public override IApiController<UserLoginDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new UserLoginApiController(serviceProvider.GetRequiredService<IUserLoginApiService>(), options);
        }

        public override IApiClient<UserLoginDto> GetClient(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                })
                .Build();

            return new UserLoginApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiClient<UserLoginDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();

            return new UserLoginApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiService<UserLoginDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IUserLoginApiService>();
        }

        public override void UpdateObject(UserLoginDto dto)
        {
            dto.ProviderDisplayName = Guid.NewGuid().ToString();
        }

        public override UserLoginDto FindObject(List<UserLoginDto> list, UserLoginDto dto)
        {
            return list.Where(x =>
                x.LoginProvider == dto.LoginProvider &&
                x.ProviderKey == dto.ProviderKey).FirstOrDefault();
        }

        public override void ValidateObjects(UserLoginDto clientDto, UserLoginDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.LoginProvider == clientDto.LoginProvider);

            Assert.True(serviceDto.ProviderKey == clientDto.ProviderKey);

            Assert.True(serviceDto.ProviderDisplayName == clientDto.ProviderDisplayName);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(UserLoginDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserLoginDto.LoginProvider), dto.LoginProvider);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserLoginDto.ProviderDisplayName), dto.ProviderDisplayName);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserLoginDto.ProviderKey), dto.ProviderKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserLoginDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserLoginDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}