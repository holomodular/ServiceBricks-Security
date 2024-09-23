using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user registers.
    /// </summary>
    public sealed class UserRegisterRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;
        private readonly ApplicationOptions _options;
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="auditUserApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="mapper"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="options"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="serviceBus"></param>
        /// <param name="configuration"></param>
        public UserRegisterRule(
            ILoggerFactory loggerFactory,
            IUserAuditApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService,
            IOptions<ApplicationOptions> options,
            IBusinessRuleService businessRuleService,
            IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UserRegisterRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerApiService;
            _iPAddressService = iPAddressService;
            _options = options.Value;
            _businessRuleService = businessRuleService;
            _configuration = configuration;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserRegisterProcess),
                typeof(UserRegisterRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(UserRegisterProcess),
                typeof(UserRegisterRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                // AI: Make sure the context object is the correct type
                var e = context.Object as UserRegisterProcess;
                if (e == null)
                    return response;

                // AI: create user object
                var nowDate = DateTimeOffset.UtcNow;
                UserDto user = new UserDto()
                {
                    Email = e.Email,
                    UserName = e.Email,
                    EmailConfirmed = e.EmailConfirmed,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    CreateDate = nowDate,
                    UpdateDate = nowDate,
                    PhoneNumberConfirmed = false,
                    NormalizedEmail = e.Email.ToUpper(),
                    NormalizedUserName = e.Email.ToUpper(),
                };

                // AI: Call usermanager to create user
                var respUser = _userManagerService.Create(
                    user,
                    e.Password);
                if (respUser.Error)
                {
                    response.CopyFrom(respUser);
                    return response;
                }

                // AI: Add user to user role
                var respAddRole = _userManagerService.AddToRole(
                    respUser.Item.StorageKey,
                    ServiceBricksConstants.SECURITY_ROLE_USER_NAME);

                // Add Claims
                //var respAddClaim = await _userManagerService.AddClaimAsync(
                //    respUser.Item.StorageKey,
                //    new Claim(ClaimTypes.Email, e.Email));
                //respAddClaim = await _userManagerService.AddClaimAsync(
                //    respUser.Item.StorageKey,
                //    new Claim(TimezoneService.CLAIM_TIMEZONE, e.TimezoneName));

                // AI: Determine if we need to send an email
                if (e.CreateEmail)
                {
                    // AI: Generate email confirmation code
                    var respCode = _userManagerService.GenerateEmailConfirmationToken(respUser.Item.StorageKey);
                    if (respCode.Error)
                    {
                        response.CopyFrom(respCode);
                        return response;
                    }

                    // AI: Create the callback URL
                    string encodedConfirmCode = HttpUtility.UrlEncode(respCode.Item);
                    string baseUrl = _configuration.GetValue<string>(SecurityConstants.APPSETTING_SECURITY_CALLBACKURL);
                    string callbackUrl = string.Format(
                            "{0}/ConfirmEmail?code={1}&userId={2}",
                            baseUrl, encodedConfirmCode, respUser.Item.StorageKey);

                    // AI: Send the confirm email process
                    SendConfirmEmailProcess sendProcess = new SendConfirmEmailProcess(
                        respUser.Item, callbackUrl);
                    var respSend = _businessRuleService.ExecuteProcess(sendProcess);
                }

                // AI: Audit user
                _auditUserApiService.Create(new UserAuditDto()
                {
                    AuditType = AuditType.REGISTER_TEXT,
                    RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                    UserStorageKey = respUser.Item.StorageKey,
                    IPAddress = _iPAddressService.GetIPAddress()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return response;
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                // AI: Make sure the context object is the correct type
                var e = context.Object as UserRegisterProcess;
                if (e == null)
                    return response;

                // AI: create user object
                var nowDate = DateTimeOffset.UtcNow;
                UserDto user = new UserDto()
                {
                    Email = e.Email,
                    UserName = e.Email,
                    EmailConfirmed = e.EmailConfirmed,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    CreateDate = nowDate,
                    UpdateDate = nowDate,
                    PhoneNumberConfirmed = false,
                    NormalizedEmail = e.Email.ToUpper(),
                    NormalizedUserName = e.Email.ToUpper(),
                };

                // AI: Call usermanager to create user
                var respUser = await _userManagerService.CreateAsync(
                    user,
                    e.Password);
                if (respUser.Error)
                {
                    response.CopyFrom(respUser);
                    return response;
                }

                // AI: Add user to user role
                var respAddRole = await _userManagerService.AddToRoleAsync(
                    respUser.Item.StorageKey,
                    ServiceBricksConstants.SECURITY_ROLE_USER_NAME);

                // Add Claims
                //var respAddClaim = await _userManagerService.AddClaimAsync(
                //    respUser.Item.StorageKey,
                //    new Claim(ClaimTypes.Email, e.Email));
                //respAddClaim = await _userManagerService.AddClaimAsync(
                //    respUser.Item.StorageKey,
                //    new Claim(TimezoneService.CLAIM_TIMEZONE, e.TimezoneName));

                // AI: Determine if we need to send an email
                if (e.CreateEmail)
                {
                    // AI: Generate email confirmation code
                    var respCode = await _userManagerService.GenerateEmailConfirmationTokenAsync(respUser.Item.StorageKey);
                    if (respCode.Error)
                    {
                        response.CopyFrom(respCode);
                        return response;
                    }

                    // AI: Create the callback URL
                    string encodedConfirmCode = HttpUtility.UrlEncode(respCode.Item);
                    string baseUrl = _configuration.GetValue<string>(SecurityConstants.APPSETTING_SECURITY_CALLBACKURL);
                    string callbackUrl = string.Format(
                            "{0}/ConfirmEmail?code={1}&userId={2}",
                            baseUrl, encodedConfirmCode, respUser.Item.StorageKey);

                    // AI: Send the confirm email process
                    SendConfirmEmailProcess sendProcess = new SendConfirmEmailProcess(
                        respUser.Item, callbackUrl);
                    var respSend = await _businessRuleService.ExecuteProcessAsync(sendProcess);
                }

                // AI: Audit user
                await _auditUserApiService.CreateAsync(new UserAuditDto()
                {
                    AuditType = AuditType.REGISTER_TEXT,
                    RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                    UserStorageKey = respUser.Item.StorageKey,
                    IPAddress = _iPAddressService.GetIPAddress()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return response;
        }
    }
}