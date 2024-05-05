using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    public class ApplicationRoleApiClient : ApiClient<ApplicationRoleDto>, IApplicationRoleApiClient
    {
        protected readonly IConfiguration _configuration;

        public ApplicationRoleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/ApplicationRole";
        }
    }
}