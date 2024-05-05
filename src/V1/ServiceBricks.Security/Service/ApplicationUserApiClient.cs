using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    public class ApplicationUserApiClient : ApiClient<ApplicationUserDto>, IApplicationUserApiClient
    {
        protected readonly IConfiguration _configuration;

        public ApplicationUserApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/ApplicationUser";
        }
    }
}