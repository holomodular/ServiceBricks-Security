using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbUserRoleTestManager : UserRoleTestManager
    {
        public override UserRoleDto GetObjectNotFound()
        {
            return new UserRoleDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class UserRoleTestManager : TestManager<UserRoleDto>
    {
        public virtual UserDto ApplicationUser { get; set; }
        public virtual RoleDto ApplicationRole { get; set; }
        public virtual RoleDto ApplicationRole2 { get; set; }

        public override UserRoleDto GetMinimumDataObject()
        {
            return new UserRoleDto()
            {
                RoleStorageKey = ApplicationRole.StorageKey,
                UserStorageKey = ApplicationUser.StorageKey
            };
        }

        public override UserRoleDto GetMaximumDataObject()
        {
            var model = new UserRoleDto()
            {
                RoleStorageKey = ApplicationRole2.StorageKey,
                UserStorageKey = ApplicationUser.StorageKey
            };
            return model;
        }

        public override IApiController<UserRoleDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new UserRoleApiController(serviceProvider.GetRequiredService<IUserRoleApiService>(), options);
        }

        public override IApiController<UserRoleDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new UserRoleApiController(serviceProvider.GetRequiredService<IUserRoleApiService>(), options);
        }

        public override IApiClient<UserRoleDto> GetClient(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                })
                .Build();

            return new UserRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiClient<UserRoleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();

            return new UserRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiService<UserRoleDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IUserRoleApiService>();
        }

        public override void UpdateObject(UserRoleDto dto)
        {
            //nothing to update, all keys
        }

        public override void ValidateObjects(UserRoleDto clientDto, UserRoleDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.RoleStorageKey == clientDto.RoleStorageKey);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(UserRoleDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserRoleDto.RoleStorageKey), dto.RoleStorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserRoleDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserRoleDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}