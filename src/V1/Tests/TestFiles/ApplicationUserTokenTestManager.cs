using Microsoft.Extensions.DependencyInjection;
using ServiceQuery;
using ServiceBricks.Security;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationUserTokenTestManager : ApplicationUserTokenTestManager
    {
        public override ApplicationUserTokenDto GetObjectNotFound()
        {
            return new ApplicationUserTokenDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationUserTokenTestManager : TestManager<ApplicationUserTokenDto>
    {
        public virtual ApplicationUserDto ApplicationUser { get; set; }

        public override ApplicationUserTokenDto GetMinimumDataObject()
        {
            return new ApplicationUserTokenDto()
            {
                UserStorageKey = ApplicationUser.StorageKey,
                LoginProvider = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
        }

        public override ApplicationUserTokenDto GetMaximumDataObject()
        {
            var model = new ApplicationUserTokenDto()
            {
                UserStorageKey = ApplicationUser.StorageKey,
                LoginProvider = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString()
            };
            return model;
        }

        public override IApiController<ApplicationUserTokenDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationUserTokenApiController(serviceProvider.GetRequiredService<IApplicationUserTokenApiService>(), options);
        }

        public override IApiController<ApplicationUserTokenDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationUserTokenApiController(serviceProvider.GetRequiredService<IApplicationUserTokenApiService>(), options);
        }

        public override IApiClient<ApplicationUserTokenDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationUserTokenApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationUserTokenDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationUserTokenApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationUserTokenDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationUserTokenApiService>();
        }

        public override void UpdateObject(ApplicationUserTokenDto dto)
        {
            dto.Value = Guid.NewGuid().ToString();
        }

        public override ApplicationUserTokenDto FindObject(List<ApplicationUserTokenDto> list, ApplicationUserTokenDto dto)
        {
            return list.Where(x =>
            x.UserStorageKey == dto.UserStorageKey &&
            x.LoginProvider == dto.LoginProvider &&
            x.Name == dto.Name).FirstOrDefault();
        }

        public override void ValidateObjects(ApplicationUserTokenDto clientDto, ApplicationUserTokenDto serviceDto, HttpMethod method)
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

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationUserTokenDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserTokenDto.LoginProvider), dto.LoginProvider);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserTokenDto.Name), dto.Name);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserTokenDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserTokenDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserTokenDto.Value), dto.Value);
            queries.Add(qb.Build());

            return queries;
        }
    }
}