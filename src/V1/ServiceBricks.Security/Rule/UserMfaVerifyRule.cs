using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This a business rule happens when MFA is needed.
    /// </summary>
    public sealed class UserMfaVerifyRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="auditUserApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="iPAddressService"></param>
        public UserMfaVerifyRule(
            ILoggerFactory loggerFactory,
            IUserAuditApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService)
        {
            _logger = loggerFactory.CreateLogger<UserMfaVerifyRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerApiService;
            _iPAddressService = iPAddressService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserMfaVerifyProcess),
                typeof(UserMfaVerifyRule));
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
                var p = context.Object as UserMfaVerifyProcess;
                if (p == null)
                    return response;

                // AI: attempt 2FA sign in
                var result = _userManagerService.TwoFactorSignIn(
                    p.Provider,
                    p.Code,
                    p.RememberMe,
                    p.RememberBrowser);

                if (result.Error)
                {
                    response.CopyFrom(result);
                    return response;
                }

                // AI: Find the user
                var respUser = _userManagerService.GetTwoFactorAuthenticationUser();
                if (respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                // AI: Audit user
                _auditUserApiService.Create(new UserAuditDto()
                {
                    AuditType = AuditType.MFA_VERIFY_TEXT,
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
                var p = context.Object as UserMfaVerifyProcess;
                if (p == null)
                    return response;

                // AI: attempt 2FA sign in
                var result = await _userManagerService.TwoFactorSignInAsync(
                    p.Provider,
                    p.Code,
                    p.RememberMe,
                    p.RememberBrowser);

                if (result.Error)
                {
                    response.CopyFrom(result);
                    return response;
                }

                // AI: Find the user
                var respUser = await _userManagerService.GetTwoFactorAuthenticationUserAsync();
                if (respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                // AI: Audit user
                await _auditUserApiService.CreateAsync(new UserAuditDto()
                {
                    AuditType = AuditType.MFA_VERIFY_TEXT,
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