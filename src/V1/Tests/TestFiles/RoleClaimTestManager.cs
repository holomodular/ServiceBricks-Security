using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbRoleClaimTestManager : RoleClaimTestManager
    {
        public override RoleClaimDto GetObjectNotFound()
        {
            return new RoleClaimDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class RoleClaimTestManager : TestManager<RoleClaimDto>
    {
        public virtual RoleDto ApplicationRole { get; set; }

        public override RoleClaimDto GetMinimumDataObject()
        {
            return new RoleClaimDto()
            {
                RoleStorageKey = ApplicationRole.StorageKey
            };
        }

        public override RoleClaimDto GetMaximumDataObject()
        {
            var model = new RoleClaimDto()
            {
                ClaimType = Guid.NewGuid().ToString(),
                ClaimValue = Guid.NewGuid().ToString(),
                RoleStorageKey = ApplicationRole.StorageKey,
            };
            return model;
        }

        public override IApiController<RoleClaimDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new RoleClaimApiController(serviceProvider.GetRequiredService<IRoleClaimApiService>(), options);
        }

        public override IApiController<RoleClaimDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new RoleClaimApiController(serviceProvider.GetRequiredService<IRoleClaimApiService>(), options);
        }

        public override IApiClient<RoleClaimDto> GetClient(IServiceProvider serviceProvider)
        {
            return new RoleClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<RoleClaimDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new RoleClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<RoleClaimDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IRoleClaimApiService>();
        }

        public override void UpdateObject(RoleClaimDto dto)
        {
            dto.ClaimType = Guid.NewGuid().ToString();
            dto.ClaimValue = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(RoleClaimDto clientDto, RoleClaimDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.ClaimType == clientDto.ClaimType);

            Assert.True(serviceDto.ClaimValue == clientDto.ClaimValue);

            Assert.True(serviceDto.RoleStorageKey == clientDto.RoleStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(RoleClaimDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleClaimDto.ClaimType), dto.ClaimType);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleClaimDto.ClaimValue), dto.ClaimValue);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleClaimDto.RoleStorageKey), dto.RoleStorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleClaimDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}