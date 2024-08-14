using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbApplicationUserTestManager : ApplicationUserTestManager
    {
        public override ApplicationUserDto GetObjectNotFound()
        {
            return new ApplicationUserDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class ApplicationUserTestManagerPostgres : ApplicationUserTestManager
    {
        public override void ValidateObjects(ApplicationUserDto clientDto, ApplicationUserDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.AccessFailedCount == clientDto.AccessFailedCount);

            //Assert.True(serviceDto.ConcurrencyStamp == clientDto.ConcurrencyStamp);

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

            Assert.True(serviceDto.Email == clientDto.Email);

            Assert.True(serviceDto.EmailConfirmed == clientDto.EmailConfirmed);

            //Primary key rule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.LockoutEnabled == clientDto.LockoutEnabled);

            Assert.True(serviceDto.NormalizedEmail == clientDto.NormalizedEmail);

            Assert.True(serviceDto.NormalizedUserName == clientDto.NormalizedUserName);

            Assert.True(serviceDto.PasswordHash == clientDto.PasswordHash);

            Assert.True(serviceDto.PhoneNumber == clientDto.PhoneNumber);

            Assert.True(serviceDto.PhoneNumberConfirmed == clientDto.PhoneNumberConfirmed);

            Assert.True(serviceDto.SecurityStamp == clientDto.SecurityStamp);

            Assert.True(serviceDto.TwoFactorEnabled == clientDto.TwoFactorEnabled);

            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put)
                Assert.True(serviceDto.UpdateDate > clientDto.UpdateDate);
            else
            {
                // Postgres special handling
                long utcTicks = serviceDto.UpdateDate.UtcTicks;
                utcTicks = ((long)(utcTicks / 10)) * 10;
                Assert.True(utcTicks == clientDto.UpdateDate.UtcTicks);
            }

            Assert.True(serviceDto.UserName == clientDto.UserName);
        }
    }

    public class ApplicationUserTestManager : TestManager<ApplicationUserDto>
    {
        public override ApplicationUserDto GetMinimumDataObject()
        {
            return new ApplicationUserDto()
            {
                Email = Guid.NewGuid().ToString(),
            };
        }

        public override ApplicationUserDto GetMaximumDataObject()
        {
            var model = new ApplicationUserDto()
            {
                CreateDate = DateTimeOffset.UtcNow,
                AccessFailedCount = 0,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                EmailConfirmed = false,
                LockoutEnabled = false,
                LockoutEnd = DateTime.UtcNow.AddDays(1),
                NormalizedEmail = Guid.NewGuid().ToString(),
                NormalizedUserName = Guid.NewGuid().ToString(),
                PasswordHash = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                PhoneNumberConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                UpdateDate = DateTimeOffset.UtcNow,
                UserName = Guid.NewGuid().ToString()
            };
            return model;
        }

        public override IApiController<ApplicationUserDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new ApplicationUserApiController(serviceProvider.GetRequiredService<IApplicationUserApiService>(), options);
        }

        public override IApiController<ApplicationUserDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new ApplicationUserApiController(serviceProvider.GetRequiredService<IApplicationUserApiService>(), options);
        }

        public override IApiClient<ApplicationUserDto> GetClient(IServiceProvider serviceProvider)
        {
            return new ApplicationUserApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<ApplicationUserDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new ApplicationUserApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<ApplicationUserDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IApplicationUserApiService>();
        }

        public override void UpdateObject(ApplicationUserDto dto)
        {
            dto.AccessFailedCount = 1;
            dto.Email = Guid.NewGuid().ToString();
            //dto.ConcurrencyStamp = Guid.NewGuid().ToString();
            dto.EmailConfirmed = true;
            dto.LockoutEnabled = true;
            dto.LockoutEnd = DateTime.UtcNow.AddDays(1);
            dto.NormalizedEmail = Guid.NewGuid().ToString();
            dto.NormalizedUserName = Guid.NewGuid().ToString();
            dto.PasswordHash = Guid.NewGuid().ToString();
            dto.PhoneNumber = Guid.NewGuid().ToString();
            dto.PhoneNumberConfirmed = true;
            dto.SecurityStamp = Guid.NewGuid().ToString();
            dto.TwoFactorEnabled = true;
            dto.UpdateDate = DateTimeOffset.UtcNow;
            dto.UserName = Guid.NewGuid().ToString();
        }

        public override void ValidateObjects(ApplicationUserDto clientDto, ApplicationUserDto serviceDto, HttpMethod method)
        {
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.AccessFailedCount == clientDto.AccessFailedCount);

            //Assert.True(serviceDto.ConcurrencyStamp == clientDto.ConcurrencyStamp);

            //CreateDateRule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.CreateDate > clientDto.CreateDate);
            else
                Assert.True(serviceDto.CreateDate == clientDto.CreateDate);

            Assert.True(serviceDto.Email == clientDto.Email);

            Assert.True(serviceDto.EmailConfirmed == clientDto.EmailConfirmed);

            //Primary key rule
            if (method == HttpMethod.Post)
                Assert.True(serviceDto.StorageKey != clientDto.StorageKey);
            else
                Assert.True(serviceDto.StorageKey == clientDto.StorageKey);

            Assert.True(serviceDto.LockoutEnabled == clientDto.LockoutEnabled);

            Assert.True(serviceDto.NormalizedEmail == clientDto.NormalizedEmail);

            Assert.True(serviceDto.NormalizedUserName == clientDto.NormalizedUserName);

            Assert.True(serviceDto.PasswordHash == clientDto.PasswordHash);

            Assert.True(serviceDto.PhoneNumber == clientDto.PhoneNumber);

            Assert.True(serviceDto.PhoneNumberConfirmed == clientDto.PhoneNumberConfirmed);

            Assert.True(serviceDto.SecurityStamp == clientDto.SecurityStamp);

            Assert.True(serviceDto.TwoFactorEnabled == clientDto.TwoFactorEnabled);

            //UpdateDateRule
            if (method == HttpMethod.Post || method == HttpMethod.Put)
                Assert.True(serviceDto.UpdateDate > clientDto.UpdateDate);
            else
                Assert.True(serviceDto.UpdateDate == clientDto.UpdateDate);

            Assert.True(serviceDto.UserName == clientDto.UserName);
        }

        public override List<ServiceQueryRequest> GetQueriesForObject(ApplicationUserDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                 IsEqual(nameof(ApplicationUserDto.AccessFailedCount), dto.AccessFailedCount.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserDto.ConcurrencyStamp), dto.ConcurrencyStamp);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserDto.CreateDate), dto.CreateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(ApplicationUserDto.Email), dto.Email);
            queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.EmailConfirmed), dto.EmailConfirmed.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.LockoutEnabled), dto.LockoutEnabled.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.LockoutEnd), dto.LockoutEnd.Value.ToString("o"));
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.NormalizedEmail), dto.NormalizedEmail);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.NormalizedUserName), dto.NormalizedUserName);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.PasswordHash), dto.PasswordHash);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.PhoneNumber), dto.PhoneNumber);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.PhoneNumberConfirmed), dto.PhoneNumberConfirmed.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.SecurityStamp), dto.SecurityStamp);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.StorageKey), dto.StorageKey);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.TwoFactorEnabled), dto.TwoFactorEnabled.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.UpdateDate), dto.UpdateDate.ToString("o"));
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(ApplicationUserDto.UserName), dto.UserName);
            //queries.Add(qb.Build());

            return queries;
        }
    }
}