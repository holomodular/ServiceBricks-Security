using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API client for the Authentication entity.
    /// </summary>
    public partial class AuthenticationApiClient : ServiceClient, IAuthenticationApiClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        /// <exception cref="Exception"></exception>
        public AuthenticationApiClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(httpClientFactory)
        {
            var config = configuration.GetApiConfig(SecurityConstants.APPSETTING_CLIENT_APICONFIG);
            if (config == null)
                throw new Exception("ApiConfig not found");
            ApiResource = config.BaseServiceUrl + @"Security/Authentication";
        }

        /// <summary>
        /// The APIResournce
        /// </summary>
        public virtual string ApiResource { get; set; }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<AccessTokenResponse> AuthenticateUser(AccessTokenRequest request)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiResource);
            var data = request != null ? JsonConvert.SerializeObject(request) : string.Empty;
            req.Content = new StringContent(data, System.Text.Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            var result = Send(req);
            return GetAccessTokenAsync(result).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<AccessTokenResponse>> AuthenticateUserAsync(AccessTokenRequest request)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiResource + "Async");
            var data = request != null ? JsonConvert.SerializeObject(request) : string.Empty;
            req.Content = new StringContent(data, System.Text.Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            var result = await SendAsync(req);
            return await GetAccessTokenAsync(result);
        }

        /// <summary>
        /// Get the access token
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
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