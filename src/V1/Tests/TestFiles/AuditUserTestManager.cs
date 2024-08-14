using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbAuditUserTestManager : AuditUserTestManager
    {
        public override AuditUserDto GetObjectNotFound()
        {
            return new AuditUserDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class AuditUserTestManagerPostgres : AuditUserTestManager
    {
        public override void ValidateObjects(AuditUserDto clientDto, AuditUserDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else if (method == HttpMethod.Get)
            {
                // Postgres special handling
                long utcTicks = serviceDto.CreateDate.UtcTicks;
                utcTicks = ((long)(utcTicks / 10)) * 10;
                Assert.True(utcTicks == clientDto.CreateDate.UtcTicks);
            }
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);

            Assert.True(serviceDto.Data == clientDto.Data);

            Assert.True(serviceDto.IPAddress == clientDto.IPAddress);

            Assert.True(serviceDto.AuditName == clientDto.AuditName);

            Assert.True(serviceDto.UserAgent == clientDto.UserAgent);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }
    }

    public class AuditUserTestManager : TestManager<AuditUserDto>
    {
        public virtual ApplicationUserDto ApplicationUser { get; set; }

        public override AuditUserDto GetMinimumDataObject()
        {
            return new AuditUserDto()
            {
                UserStorageKey = ApplicationUser.StorageKey
            };
        }

        public override AuditUserDto GetMaximumDataObject()
        {
            var model = new AuditUserDto()
            {
                AuditName = Guid.NewGuid().ToString(),
                CreateDate = DateTimeOffset.UtcNow,
                Data = Guid.NewGuid().ToString(),
                IPAddress = "111.111.111.111",
                UserAgent = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey
            };
            return model;
        }

        public override IApiController<AuditUserDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new AuditUserApiController(serviceProvider.GetRequiredService<IAuditUserApiService>(), options);
        }

        public override IApiController<AuditUserDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new AuditUserApiController(serviceProvider.GetRequiredService<IAuditUserApiService>(), options);
        }

        public override IApiClient<AuditUserDto> GetClient(IServiceProvider serviceProvider)
        {
            return new AuditUserApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<AuditUserDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new AuditUserApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<AuditUserDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IAuditUserApiService>();
        }

        public override void UpdateObject(AuditUserDto dto)
        {
            dto.UserAgent = Guid.NewGuid().ToString();
            dto.Data = Guid.NewGuid().ToString();
            dto.IPAddress = "222.222.222.222";
            dto.AuditName = Guid.NewGuid().ToString();
            dto.UserStorageKey = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(AuditUserDto clientDto, AuditUserDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);

            Assert.True(serviceDto.Data == clientDto.Data);

            Assert.True(serviceDto.IPAddress == clientDto.IPAddress);

            Assert.True(serviceDto.AuditName == clientDto.AuditName);

            Assert.True(serviceDto.UserAgent == clientDto.UserAgent);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(AuditUserDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.UserAgent), dto.UserAgent);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.Data), dto.Data);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.CreateDate), dto.CreateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.IPAddress), dto.IPAddress);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.AuditName), dto.AuditName);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(AuditUserDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}