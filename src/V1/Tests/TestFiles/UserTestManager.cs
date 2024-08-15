using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceBricks.Security;
using ServiceQuery;

namespace ServiceBricks.Xunit
{
    public class MongoDbUserTestManager : UserTestManager
    {
        public override UserDto GetObjectNotFound()
        {
            return new UserDto()
            {
                StorageKey = "000000000000000000000000"
            };
        }
    }

    public class UserTestManagerPostgres : UserTestManager
    {
        public override void ValidateObjects(UserDto clientDto, UserDto serviceDto, HttpMethod method)
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

    public class UserTestManager : TestManager<UserDto>
    {
        public override UserDto GetMinimumDataObject()
        {
            return new UserDto()
            {
                Email = Guid.NewGuid().ToString(),
            };
        }

        public override UserDto GetMaximumDataObject()
        {
            var model = new UserDto()
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

        public override IApiController<UserDto> GetController(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = false, ExposeSystemErrors = true });
            return new UserApiController(serviceProvider.GetRequiredService<IUserApiService>(), options);
        }

        public override IApiController<UserDto> GetControllerReturnResponse(IServiceProvider serviceProvider)
        {
            var options = new OptionsWrapper<ApiOptions>(new ApiOptions() { ReturnResponseObject = true, ExposeSystemErrors = true });
            return new UserApiController(serviceProvider.GetRequiredService<IUserApiService>(), options);
        }

        public override IApiClient<UserDto> GetClient(IServiceProvider serviceProvider)
        {
            return new UserApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiClient<UserDto> GetClientReturnResponse(IServiceProvider serviceProvider)
        {
            return new UserApiClient(
                serviceProvider.GetRequiredService<ILoggerFactory>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IConfiguration>());
        }

        public override IApiService<UserDto> GetService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IUserApiService>();
        }

        public override void UpdateObject(UserDto dto)
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

        public override void ValidateObjects(UserDto clientDto, UserDto serviceDto, HttpMethod method)
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

        public override List<ServiceQueryRequest> GetQueriesForObject(UserDto dto)
        {
            List<ServiceQueryRequest> queries = new List<ServiceQueryRequest>();

            var qb = ServiceQueryRequestBuilder.New().
                 IsEqual(nameof(UserDto.AccessFailedCount), dto.AccessFailedCount.ToString());
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserDto.ConcurrencyStamp), dto.ConcurrencyStamp);
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserDto.CreateDate), dto.CreateDate.ToString("o"));
            queries.Add(qb.Build());

            qb = ServiceQueryRequestBuilder.New().
                IsEqual(nameof(UserDto.Email), dto.Email);
            queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.EmailConfirmed), dto.EmailConfirmed.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.LockoutEnabled), dto.LockoutEnabled.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.LockoutEnd), dto.LockoutEnd.Value.ToString("o"));
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.NormalizedEmail), dto.NormalizedEmail);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.NormalizedUserName), dto.NormalizedUserName);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.PasswordHash), dto.PasswordHash);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.PhoneNumber), dto.PhoneNumber);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.PhoneNumberConfirmed), dto.PhoneNumberConfirmed.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.SecurityStamp), dto.SecurityStamp);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.StorageKey), dto.StorageKey);
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.TwoFactorEnabled), dto.TwoFactorEnabled.ToString());
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.UpdateDate), dto.UpdateDate.ToString("o"));
            //queries.Add(qb.Build());

            //qb = ServiceQueryRequestBuilder.New().
            //    IsEqual(nameof(UserDto.UserName), dto.UserName);
            //queries.Add(qb.Build());

            return queries;
        }
    }
}