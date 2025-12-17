using Microsoft.Extensions.Configuration;

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
            var config = configuration.GetApiConfig(SecurityModelConstants.APPSETTING_CLIENT_APICONFIG);
            if (config == null)
                throw new Exception("ApiConfig not found");
            ApiResource = config.BaseServiceUrl + @"/Security/Authentication/AuthenticateUser";
            if (config.ReturnResponseObject)
                ApiResponseFormat = ApiResponseFormat.modern;
            else
                ApiResponseFormat = ApiResponseFormat.classic;
        }

        /// <summary>
        /// The APIResournce
        /// </summary>
        public virtual string ApiResource { get; set; }

        /// <summary>
        /// The response format
        /// </summary>
        public virtual ApiResponseFormat ApiResponseFormat { get; set; }


        /// <summary>
        /// Get the response format
        /// </summary>
        /// <returns></returns>
        protected virtual string GetResponseFormat()
        {
            switch (ApiResponseFormat)
            {
                default:
                case ApiResponseFormat.modern:
                    return "modern";

                case ApiResponseFormat.classic:
                    return "classic";
            }
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<AccessTokenResponse> AuthenticateUser(AccessTokenRequest request)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiResource + $"?format={GetResponseFormat()}");
            var data = request != null ? JsonSerializer.Instance.SerializeObject(request) : string.Empty;
            req.Content = new StringContent(data, System.Text.Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            var result = Send(req);
            return GetAccessToken(result);
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<AccessTokenResponse>> AuthenticateUserAsync(AccessTokenRequest request)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, ApiResource + $"Async?format={GetResponseFormat()}");
            var data = request != null ? JsonSerializer.Instance.SerializeObject(request) : string.Empty;
            req.Content = new StringContent(data, System.Text.Encoding.UTF8, CONTENTTYPE_APPLICATIONJSON);
            var result = await SendAsync(req);
            return await GetAccessTokenAsync(result);
        }

        /// <summary>
        /// Get the access token
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual ResponseItem<AccessTokenResponse> GetAccessToken(HttpResponseMessage result)
        {
            ResponseItem<AccessTokenResponse> resp = new ResponseItem<AccessTokenResponse>();
            string content = string.Empty;
            if (result.Content != null)
            {
                using (var stream = result.Content.ReadAsStream())
                using (var reader = new StreamReader(stream))
                        content = reader.ReadToEnd();                                    
            }
            if (result.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    if(ApiResponseFormat == ApiResponseFormat.modern)
                        resp = JsonSerializer.Instance.DeserializeObject<ResponseItem<AccessTokenResponse>>(content);
                    else
                        resp.Item = JsonSerializer.Instance.DeserializeObject<AccessTokenResponse>(content);
                }
                    
                return resp;
            }
            else
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                if (!string.IsNullOrEmpty(content))
                    resp.AddMessage(ResponseMessage.CreateError(new Exception(content), LocalizationResource.ERROR_REST_CLIENT));
                
                return resp;
            }
        }

        /// <summary>
        /// Get the access token
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual async Task<ResponseItem<AccessTokenResponse>> GetAccessTokenAsync(HttpResponseMessage result)
        {
            ResponseItem<AccessTokenResponse> resp = new ResponseItem<AccessTokenResponse>();
            string content = string.Empty;
            if(result.Content != null)
                content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {                
                if (!string.IsNullOrEmpty(content))
                {
                    if (ApiResponseFormat == ApiResponseFormat.modern)
                        resp = JsonSerializer.Instance.DeserializeObject<ResponseItem<AccessTokenResponse>>(content);
                    else
                        resp.Item = JsonSerializer.Instance.DeserializeObject<AccessTokenResponse>(content);
                }
                return resp;
            }
            else
            {
                resp.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_REST_CLIENT));
                if (!string.IsNullOrEmpty(content))
                    resp.AddMessage(ResponseMessage.CreateError(new Exception(content), LocalizationResource.ERROR_REST_CLIENT));
                return resp;
            }
        }
    }
}