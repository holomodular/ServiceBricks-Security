using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserClaim entity.
    /// </summary>
    public partial class UserClaimApiClient : ApiClient<UserClaimDto>, IUserClaimApiClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public UserClaimApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityModelConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/UserClaim";
        }
    }
}