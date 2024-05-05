using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;
using ServiceBricks.Security;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationUserRoleTestManager : ApplicationUserRoleTestManager
    {
        public override ApplicationUserRoleDto GetObjectNotFound()
        {
            return new ApplicationUserRoleDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationUserRoleTestManager : TestManager<ApplicationUserRoleDto>
    {
        public virtual ApplicationUserDto ApplicationUser { get; set; }
        public virtual ApplicationRoleDto ApplicationRole { get; set; }
        public virtual ApplicationRoleDto ApplicationRole2 { get; set; }

        public override ApplicationUserRoleDto GetMinimumDataObject()
        {
            return new ApplicationUserRoleDto()
            {
                RoleStorageKey = ApplicationRole.StorageKey,
                UserStorageKey = ApplicationUser.StorageKey
            };
        }

        public override ApplicationUserRoleDto GetMaximumDataObject()
        {
            var model = new ApplicationUserRoleDto()
            {
                RoleStorageKey = ApplicationRole2.StorageKey,
                UserStorageKey = ApplicationUser.StorageKey
            };
            return model;
        }

        public override IApiController<ApplicationUserRoleDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationUserRoleApiController(serviceProvider.GetRequiredService<IApplicationUserRoleApiService>(), options);
        }

        public override IApiController<ApplicationUserRoleDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationUserRoleApiController(serviceProvider.GetRequiredService<IApplicationUserRoleApiService>(), options);
        }

        public override IApiClient<ApplicationUserRoleDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationUserRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationUserRoleDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationUserRoleApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationUserRoleDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationUserRoleApiService>();
        }

        public override void UpdateObject(ApplicationUserRoleDto dto)
        {
            //nothing to update, all keys
        }

        public override void ValidateObjects(ApplicationUserRoleDto clientDto, ApplicationUserRoleDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.RoleStorageKey == clientDto.RoleStorageKey);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationUserRoleDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserRoleDto.RoleStorageKey), dto.RoleStorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserRoleDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserRoleDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}