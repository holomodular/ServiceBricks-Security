using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbUserTokenTestManager : UserTokenTestManager
    {
        public override UserTokenDto GetObjectNotFound()
        {
            return new UserTokenDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class UserTokenTestManager : TestManager<UserTokenDto>
    {
        public virtual UserDto ApplicationUser { get; set; }

        public override UserTokenDto GetMinimumDataObject()
        {
            return new UserTokenDto()
            {
                UserStorageKey = ApplicationUser.StorageKey,
                LoginProvider = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
        }

        public override UserTokenDto GetMaximumDataObject()
        {
            var model = new UserTokenDto()
            {
                UserStorageKey = ApplicationUser.StorageKey,
                LoginProvider = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString()
            };
            return model;
        }

        public override IApiController<UserTokenDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new UserTokenApiController(serviceProvider.GetRequiredService<IUserTokenApiService>(), options);
        }

        public override IApiController<UserTokenDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new UserTokenApiController(serviceProvider.GetRequiredService<IUserTokenApiService>(), options);
        }

        public override IApiClient<UserTokenDto> GetClient(IServiceProvider serviceProvider)
        {
            return new UserTokenApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<UserTokenDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new UserTokenApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<UserTokenDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IUserTokenApiService>();
        }

        public override void UpdateObject(UserTokenDto dto)
        {
            dto.Value = Guid.NewGuid().ToString();
        }

        public override UserTokenDto FindObject(List<UserTokenDto> list, UserTokenDto dto)
        {
            return list.Where(x =>
            x.UserStorageKey == dto.UserStorageKey &&
            x.LoginProvider == dto.LoginProvider &&
            x.Name == dto.Name).FirstOrDefault();
        }

        public override void ValidateObjects(UserTokenDto clientDto, UserTokenDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.LoginProvider == clientDto.LoginProvider);

            Assert.True(serviceDto.Name == clientDto.Name);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);

            Assert.True(serviceDto.Value == clientDto.Value);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(UserTokenDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserTokenDto.LoginProvider), dto.LoginProvider);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserTokenDto.Name), dto.Name);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserTokenDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserTokenDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserTokenDto.Value), dto.Value);
            queries.Add(qb.Build());

            return queries;
        }
    }
}