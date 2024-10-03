using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationUserClaimTestManager : ApplicationUserClaimTestManager
    {
        public override UserClaimDto GetObjectNotFound()
        {
            return new UserClaimDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationUserClaimTestManager : TestManager<UserClaimDto>
    {
        public virtual UserDto ApplicationUser { get; set; }

        public override UserClaimDto GetMinimumDataObject()
        {
            return new UserClaimDto()
            {
                UserStorageKey = ApplicationUser.StorageKey
            };
        }

        public override UserClaimDto GetMaximumDataObject()
        {
            var model = new UserClaimDto()
            {
                ClaimType = Guid.NewGuid().ToString(),
                ClaimValue = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey
            };
            return model;
        }

        public override IApiController<UserClaimDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new UserClaimApiController(serviceProvider.GetRequiredService<IUserClaimApiService>(), options);
        }

        public override IApiController<UserClaimDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new UserClaimApiController(serviceProvider.GetRequiredService<IUserClaimApiService>(), options);
        }

        public override IApiClient<UserClaimDto> GetClient(IServiceProvider serviceProvider)
        {
            return new UserClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<UserClaimDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new UserClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<UserClaimDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IUserClaimApiService>();
        }

        public override void UpdateObject(UserClaimDto dto)
        {
            dto.ClaimType = Guid.NewGuid().ToString();
            dto.ClaimValue = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(UserClaimDto clientDto, UserClaimDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.ClaimType == clientDto.ClaimType);

            Assert.True(serviceDto.ClaimValue == clientDto.ClaimValue);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(UserClaimDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserClaimDto.ClaimType), dto.ClaimType);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserClaimDto.ClaimValue), dto.ClaimValue);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserClaimDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserClaimDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}