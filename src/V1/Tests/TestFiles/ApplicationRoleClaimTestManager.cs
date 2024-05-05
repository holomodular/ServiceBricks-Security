using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;
using ServiceBricks.Security;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationRoleClaimTestManager : ApplicationRoleClaimTestManager
    {
        public override ApplicationRoleClaimDto GetObjectNotFound()
        {
            return new ApplicationRoleClaimDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationRoleClaimTestManager : TestManager<ApplicationRoleClaimDto>
    {
        public virtual ApplicationRoleDto ApplicationRole { get; set; }

        public override ApplicationRoleClaimDto GetMinimumDataObject()
        {
            return new ApplicationRoleClaimDto()
            {
                RoleStorageKey = ApplicationRole.StorageKey
            };
        }

        public override ApplicationRoleClaimDto GetMaximumDataObject()
        {
            var model = new ApplicationRoleClaimDto()
            {
                ClaimType = Guid.NewGuid().ToString(),
                ClaimValue = Guid.NewGuid().ToString(),
                RoleStorageKey = ApplicationRole.StorageKey,
            };
            return model;
        }

        public override IApiController<ApplicationRoleClaimDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationRoleClaimApiController(serviceProvider.GetRequiredService<IApplicationRoleClaimApiService>(), options);
        }

        public override IApiController<ApplicationRoleClaimDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationRoleClaimApiController(serviceProvider.GetRequiredService<IApplicationRoleClaimApiService>(), options);
        }

        public override IApiClient<ApplicationRoleClaimDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationRoleClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationRoleClaimDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationRoleClaimApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationRoleClaimDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationRoleClaimApiService>();
        }

        public override void UpdateObject(ApplicationRoleClaimDto dto)
        {
            dto.ClaimType = Guid.NewGuid().ToString();
            dto.ClaimValue = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ApplicationRoleClaimDto clientDto, ApplicationRoleClaimDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.ClaimType == clientDto.ClaimType);

            Assert.True(serviceDto.ClaimValue == clientDto.ClaimValue);

            Assert.True(serviceDto.RoleStorageKey == clientDto.RoleStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationRoleClaimDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleClaimDto.ClaimType), dto.ClaimType);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleClaimDto.ClaimValue), dto.ClaimValue);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleClaimDto.RoleStorageKey), dto.RoleStorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleClaimDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}