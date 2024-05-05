using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;
using ServiceBricks.Security;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationRoleTestManager : ApplicationRoleTestManager
    {
        public override ApplicationRoleDto GetObjectNotFound()
        {
            return new ApplicationRoleDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationRoleTestManager : TestManager<ApplicationRoleDto>
    {
        public override ApplicationRoleDto GetMaximumDataObject()
        {
            var model = new ApplicationRoleDto()
            {
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                NormalizedName = Guid.NewGuid().ToString()
            };
            return model;
        }

        public override IApiController<ApplicationRoleDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationRoleApiController(serviceProvider.GetRequiredService<IApplicationRoleApiService>(), options);
        }

        public override IApiController<ApplicationRoleDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationRoleApiController(serviceProvider.GetRequiredService<IApplicationRoleApiService>(), options);
        }

        public override IApiClient<ApplicationRoleDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationRoleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationRoleDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationRoleApiService>();
        }

        public override void UpdateObject(ApplicationRoleDto dto)
        {
            dto.Name = Guid.NewGuid().ToString();
            dto.NormalizedName = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ApplicationRoleDto clientDto, ApplicationRoleDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            //Assert.True(serviceDto.ConcurrencyStamp == clientDto.ConcurrencyStamp);

            Assert.True(serviceDto.Name == clientDto.Name);

            Assert.True(serviceDto.NormalizedName == clientDto.NormalizedName);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationRoleDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleDto.ConcurrencyStamp), dto.ConcurrencyStamp);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleDto.Name), dto.Name);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleDto.NormalizedName), dto.NormalizedName);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationRoleDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}