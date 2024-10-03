using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceQuery;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This is a REST API service for the Authentication entity.
    /// </summary>
    public partial class AuthenticationApiService : IAuthenticationApiService
    {
        protected readonly IConfiguration _configuration;
        protected readonly SecurityTokenOptions _securityOptions;
        protected readonly IUserManagerService _userManagerService;
        protected readonly IUserClaimApiService _applicationUserClaimApiService;
        protected readonly IUserRoleApiService _applicationUserRoleApiService;
        protected readonly IRoleClaimApiService _applicationRoleClaimApiService;
        protected readonly IRoleApiService _applicationRoleApiService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ApiOptions _apiOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="securityOptions"></param>
        /// <param name="configuration"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="applicationUserClaimApiService"></param>
        /// <param name="applicationUserRoleApiService"></param>
        /// <param name="applicationRoleClaimApiService"></param>
        /// <param name="applicationRoleApiService"></param>
        /// <param name="httpContextAccessor"></param>
        public AuthenticationApiService(
            IOptions<SecurityTokenOptions> securityOptions,
            IConfiguration configuration,
            IUserManagerService userManagerApiService,
            IUserClaimApiService applicationUserClaimApiService,
            IUserRoleApiService applicationUserRoleApiService,
            IRoleClaimApiService applicationRoleClaimApiService,
            IRoleApiService applicationRoleApiService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ApiOptions> apiOptions)
        {
            _configuration = configuration;
            _securityOptions = securityOptions.Value;
            _userManagerService = userManagerApiService;
            _applicationUserClaimApiService = applicationUserClaimApiService;
            _applicationUserRoleApiService = applicationUserRoleApiService;
            _applicationRoleClaimApiService = applicationRoleClaimApiService;
            _applicationRoleApiService = applicationRoleApiService;
            _httpContextAccessor = httpContextAccessor;
            _apiOptions = apiOptions.Value;
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IResponseItem<AccessTokenResponse> AuthenticateUser(AccessTokenRequest request)
        {
            return AuthenticateUserAsync(request).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<IResponseItem<AccessTokenResponse>> AuthenticateUserAsync(AccessTokenRequest request)
        {
            var response = new ResponseItem<AccessTokenResponse>();

            // Verify password
            var respAuth = await _userManagerService.VerifyPasswordAsync(request.client_id, request.client_secret);
            if (respAuth.Error)
            {
                // Make sure system errors are not exposed
                var messages = respAuth.Messages.ToList();
                if (!_apiOptions.ExposeSystemErrors)
                    messages = messages.Where(x => x.Severity != ResponseSeverity.ErrorSystemSensitive).ToList();
                if (messages.Count == 0)
                    messages.Add(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                foreach (var msg in messages)
                    response.AddMessage(msg);
                return response;
            }

            // Create default claims
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, respAuth.Item.StorageKey),
                new Claim(ClaimTypes.Name, respAuth.Item.UserName),
                new Claim(ClaimTypes.Email, respAuth.Item.Email),
                new Claim(JwtRegisteredClaimNames.Sub, respAuth.Item.StorageKey),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Query for data needed
            var qb = ServiceQueryRequestBuilder.New().IsEqual(
                nameof(UserClaimDto.UserStorageKey), respAuth.Item.StorageKey);
            var respUserClaims = await _applicationUserClaimApiService.QueryAsync(qb.Build());
            if (respUserClaims.Item.List.Count > 0)
            {
                foreach (var c in respUserClaims.Item.List)
                    claims.Add(new Claim(c.ClaimType, c.ClaimValue));
            }
            qb = ServiceQueryRequestBuilder.New().IsEqual(
                nameof(UserRoleDto.UserStorageKey), respAuth.Item.StorageKey);
            var respUserRoles = await _applicationUserRoleApiService.QueryAsync(qb.Build());
            if (respUserRoles.Item.List.Count > 0)
            {
                var roleids = respUserRoles.Item.List.Select(x => x.RoleStorageKey).ToList();
                qb = ServiceQueryRequestBuilder.New().IsInSet(
                    nameof(RoleClaimDto.RoleStorageKey), roleids.ToArray());
                var respRoleClaims = await _applicationRoleClaimApiService.QueryAsync(qb.Build());
                if (respRoleClaims.Item.List.Count > 0)
                {
                    foreach (var c in respRoleClaims.Item.List)
                        claims.Add(new Claim(c.ClaimType, c.ClaimValue));
                }

                qb = ServiceQueryRequestBuilder.New().IsInSet(
                    nameof(RoleDto.StorageKey), roleids.ToArray());
                var respRoles = await _applicationRoleApiService.QueryAsync(qb.Build());
                if (respRoles.Item.List.Count > 0)
                {
                    foreach (var r in respRoles.Item.List)
                        claims.Add(new Claim(ClaimTypes.Role, r.Name));
                }
            }

            // Create the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _securityOptions.ValidIssuer,
                audience: _securityOptions.ValidAudience,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_securityOptions.ExpireMinutes)),
                claims: claims,
                signingCredentials: creds
            );

            // return the token to API client
            response.Item = new AccessTokenResponse()
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires_in = (int)token.ValidTo.Subtract(DateTime.UtcNow).TotalSeconds,
                token_type = JwtBearerDefaults.AuthenticationScheme,
            };
            return response;
        }
    }
}