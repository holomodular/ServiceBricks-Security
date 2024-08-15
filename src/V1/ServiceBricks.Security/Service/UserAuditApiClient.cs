using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the AuditUser entity.
    /// </summary>
    public partial class UserAuditApiClient : ApiClient<UserAuditDto>, IUserAuditApiClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public UserAuditApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/UserAudit";
        }
    }
}