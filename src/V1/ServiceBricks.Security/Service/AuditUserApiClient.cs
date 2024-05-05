using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace ServiceBricks.Security
{
    public class AuditUserApiClient : ApiClient<AuditUserDto>, IAuditUserApiClient
    {
        protected readonly IConfiguration _configuration;

        public AuditUserApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(loggerFactory, httpClientFactory, configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG))
        {
            ApiResource = @"Security/AuditUser";
        }
    }
}