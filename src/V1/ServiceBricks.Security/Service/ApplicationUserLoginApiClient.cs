using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    public class ApplicationUserLoginApiClient : ApiClient<ApplicationUserLoginDto>, IApplicationUserLoginApiClient
    {
        protected readonly IConfiguration _configuration;

        public ApplicationUserLoginApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/ApplicationUserLogin";
        }
    }
}