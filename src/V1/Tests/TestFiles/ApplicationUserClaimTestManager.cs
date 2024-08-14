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
        public override ApplicationUserClaimDto GetObjectNotFound()
        {
            return new ApplicationUserClaimDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationUserClaimTestManager : TestManager<ApplicationUserClaimDto>
    {
        public virtual ApplicationUserDto ApplicationUser { get; set; }

        public override ApplicationUserClaimDto GetMinimumDataObject()
        {
            return new ApplicationUserClaimDto()
            {
                UserStorageKey = ApplicationUser.StorageKey
            };
        }

        public override ApplicationUserClaimDto GetMaximumDataObject()
        {
            var model = new ApplicationUserClaimDto()
            {
                ClaimType = Guid.NewGuid().ToString(),
                ClaimValue = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey
            };
            return model;
        }

        public override IApiController<ApplicationUserClaimDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationUserClaimApiController(serviceProvider.GetRequiredService<IApplicationUserClaimApiService>(), options);
        }

        public override IApiController<ApplicationUserClaimDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationUserClaimApiController(serviceProvider.GetRequiredService<IApplicationUserClaimApiService>(), options);
        }

        public override IApiClient<ApplicationUserClaimDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationUserClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationUserClaimDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationUserClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationUserClaimDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationUserClaimApiService>();
        }

        public override void UpdateObject(ApplicationUserClaimDto dto)
        {
            dto.ClaimType = Guid.NewGuid().ToString();
            dto.ClaimValue = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ApplicationUserClaimDto clientDto, ApplicationUserClaimDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.ClaimType == clientDto.ClaimType);

            Assert.True(serviceDto.ClaimValue == clientDto.ClaimValue);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationUserClaimDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserClaimDto.ClaimType), dto.ClaimType);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserClaimDto.ClaimValue), dto.ClaimValue);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserClaimDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserClaimDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}