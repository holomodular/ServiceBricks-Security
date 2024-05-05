using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;
using ServiceBricks.Security;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationUserLoginTestManager : ApplicationUserLoginTestManager
    {
        public override ApplicationUserLoginDto GetObjectNotFound()
        {
            return new ApplicationUserLoginDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationUserLoginTestManager : TestManager<ApplicationUserLoginDto>
    {
        public virtual ApplicationUserDto ApplicationUser { get; set; }

        public override ApplicationUserLoginDto GetMinimumDataObject()
        {
            return new ApplicationUserLoginDto()
            {
                LoginProvider = Guid.NewGuid().ToString(),
                ProviderKey = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey,
            };
        }

        public override ApplicationUserLoginDto GetMaximumDataObject()
        {
            var model = new ApplicationUserLoginDto()
            {
                LoginProvider = Guid.NewGuid().ToString(),
                ProviderDisplayName = Guid.NewGuid().ToString(),
                ProviderKey = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey,
            };
            return model;
        }

        public override IApiController<ApplicationUserLoginDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationUserLoginApiController(serviceProvider.GetRequiredService<IApplicationUserLoginApiService>(), options);
        }

        public override IApiController<ApplicationUserLoginDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationUserLoginApiController(serviceProvider.GetRequiredService<IApplicationUserLoginApiService>(), options);
        }

        public override IApiClient<ApplicationUserLoginDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationUserLoginApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationUserLoginDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationUserLoginApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationUserLoginDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationUserLoginApiService>();
        }

        public override void UpdateObject(ApplicationUserLoginDto dto)
        {
            dto.ProviderDisplayName = Guid.NewGuid().ToString();
        }

        public override ApplicationUserLoginDto FindObject(List<ApplicationUserLoginDto> list, ApplicationUserLoginDto dto)
        {
            return list.Where(x =>
                x.LoginProvider == dto.LoginProvider &&
                x.ProviderKey == dto.ProviderKey).FirstOrDefault();
        }

        public override void ValidateObjects(ApplicationUserLoginDto clientDto, ApplicationUserLoginDto serviceDto, HttpMethod method)
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

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationUserLoginDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserLoginDto.LoginProvider), dto.LoginProvider);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserLoginDto.ProviderDisplayName), dto.ProviderDisplayName);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserLoginDto.ProviderKey), dto.ProviderKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserLoginDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserLoginDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}