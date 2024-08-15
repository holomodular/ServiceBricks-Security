using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationRole entity.
    /// </summary>
    public partial class RoleApiClient : ApiClient<RoleDto>, IRoleApiClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public RoleApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/Role";
        }
    }
}