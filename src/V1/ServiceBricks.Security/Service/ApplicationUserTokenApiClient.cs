using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    public class ApplicationUserTokenApiClient : ApiClient<ApplicationUserTokenDto>, IApplicationUserTokenApiClient
    {
        protected readonly IConfiguration _configuration;

        public ApplicationUserTokenApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/ApplicationUserToken";
        }
    }
}