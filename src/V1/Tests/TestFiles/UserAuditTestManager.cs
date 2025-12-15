using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbUserAuditTestManager : UserAuditTestManager
    {
        public override UserAuditDto GetObjectNotFound()
        {
            return new UserAuditDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class UserAuditTestManagerPostgres : UserAuditTestManager
    {
        public override void ValidateObjects(UserAuditDto clientDto, UserAuditDto serviceDto, HttpMethod method)
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

            Assert.True(serviceDto.AuditType == clientDto.AuditType);

            Assert.True(serviceDto.RequestHeaders == clientDto.RequestHeaders);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }
    }

    public class UserAuditTestManager : TestManager<UserAuditDto>
    {
        public virtual UserDto ApplicationUser { get; set; }

        public override UserAuditDto GetMinimumDataObject()
        {
            return new UserAuditDto()
            {
                UserStorageKey = ApplicationUser.StorageKey
            };
        }

        public override UserAuditDto GetMaximumDataObject()
        {
            var model = new UserAuditDto()
            {
                AuditType = Guid.NewGuid().ToString(),
                CreateDate = DateTimeOffset.UtcNow,
                Data = Guid.NewGuid().ToString(),
                IPAddress = "111.111.111.111",
                RequestHeaders = Guid.NewGuid().ToString(),
                UserStorageKey = ApplicationUser.StorageKey
            };
            return model;
        }

        public override IApiController<UserAuditDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new UserAuditApiController(serviceProvider.GetRequiredService<IUserAuditApiService>(), options);
        }

        public override IApiController<UserAuditDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new UserAuditApiController(serviceProvider.GetRequiredService<IUserAuditApiService>(), options);
        }

        public override IApiClient<UserAuditDto> GetClient(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "false" },
                })
                .Build();

            return new UserAuditApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiClient<UserAuditDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            var appconfig = serviceProvider.GetRequiredService<IConfiguration>();
            var config = new ConfigurationBuilder()
                .AddConfiguration(appconfig)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS + ":ReturnResponseObject", "true" },
                })
                .Build();

            return new UserAuditApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                config);
        }

        public override IApiService<UserAuditDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IUserAuditApiService>();
        }

        public override void UpdateObject(UserAuditDto dto)
        {
            dto.RequestHeaders = Guid.NewGuid().ToString();
            dto.Data = Guid.NewGuid().ToString();
            dto.IPAddress = "222.222.222.222";
            dto.AuditType = Guid.NewGuid().ToString();

            // Don't update user
            //dto.UserStorageKey = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(UserAuditDto clientDto, UserAuditDto serviceDto, HttpMethod method)
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

            Assert.True(serviceDto.AuditType == clientDto.AuditType);

            Assert.True(serviceDto.RequestHeaders == clientDto.RequestHeaders);

            Assert.True(serviceDto.UserStorageKey == clientDto.UserStorageKey);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(UserAuditDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.RequestHeaders), dto.RequestHeaders);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.Data), dto.Data);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.CreateDate), dto.CreateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.IPAddress), dto.IPAddress);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.AuditType), dto.AuditType);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.StorageKey), dto.StorageKey);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserAuditDto.UserStorageKey), dto.UserStorageKey);
            queries.Add(qb.Build());

            return queries;
        }
    }
}