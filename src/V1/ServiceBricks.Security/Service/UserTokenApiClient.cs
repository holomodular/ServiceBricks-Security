using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the ApplicationUserToken entity.
    /// </summary>
    public partial class UserTokenApiClient : ApiClient<UserTokenDto>, IUserTokenApiClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public UserTokenApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/UserToken";
        }
    }
}