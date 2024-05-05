using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ServiceBricks.Security
{
    public class AuthenticationApiClient : ServiceClient, IAuthenticationApiClient
    {
        protected readonly IConfiguration _configuration;

        public AuthenticationApiClient(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(httpClientFactory)
        {
            var config = configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG);
            if (config == null)
                throw new Exception("ApiConfig not found");
            ApiResource = config.BaseServiceUrl + @"Security/Authentication";
        }

        public virtual string ApiResource { get; set; }

        public virtual IResponseItem<AccessTokenResponse> AuthenticateUser(AccessTokenRequest request)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiResource);
            req.Content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            var result = Send(req);
            return GetAccessTokenAsync(result).GetAwaiter().GetResult();
        }

        public virtual async Task<IResponseItem<AccessTokenResponse>> AuthenticateUserAsync(AccessTokenRequest request)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiResource + "Async");
            req.Content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            var result = await SendAsync(req);
            return await GetAccessTokenAsync(result);
        }

        protected virtual async Task<ResponseItem<AccessTokenResponse>> GetAccessTokenAsync(HttpResponseMessage result)
        {
            ResponseItem<AccessTokenResponse> resp = new ResponseItem<AccessTokenResponse>();
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    resp.Item = JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                return resp;
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                    resp.AddMessage(ResponseMessage.CreateError(new Exception(content), LocalizationResource.ERROR_REST_CLIENT));
                else
                    resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                return resp;
            }
        }
    }
}