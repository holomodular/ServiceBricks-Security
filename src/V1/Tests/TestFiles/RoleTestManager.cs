using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbRoleTestManager : RoleTestManager
    {
        public override RoleDto GetObjectNotFound()
        {
            return new RoleDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class RoleTestManager : TestManager<RoleDto>
    {
        public override RoleDto GetMinimumDataObject()
        {
            var model = new RoleDto()
            {
                Name = Guid.NewGuid().ToString(),
                NormalizedName = Guid.NewGuid().ToString()
            };
            return model;
        }

        public override RoleDto GetMaximumDataObject()
        {
            var model = new RoleDto()
            {
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                NormalizedName = Guid.NewGuid().ToString()
            };
            return model;
        }

        public override IApiController<RoleDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new RoleApiController(serviceProvider.GetRequiredService<IRoleApiService>(), options);
        }

        public override IApiController<RoleDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new RoleApiController(serviceProvider.GetRequiredService<IRoleApiService>(), options);
        }

        public override IApiClient<RoleDto> GetClient(IServiceProvider serviceProvider)
        {
            return new RoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<RoleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new RoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<RoleDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IRoleApiService>();
        }

        public override void UpdateObject(RoleDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
            dto.NormalizedName = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(RoleDto clientDto, RoleDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            //Assert.True(serviceDto.ConcurrencyStamp == clientDto.ConcurrencyStamp);

            Assert.True(serviceDto.Name == clientDto.Name);

            Assert.True(serviceDto.NormalizedName == clientDto.NormalizedName);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(RoleDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleDto.ConcurrencyStamp), dto.ConcurrencyStamp);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleDto.Name), dto.Name);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleDto.NormalizedName), dto.NormalizedName);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(RoleDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}