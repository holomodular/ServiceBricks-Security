using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule creates a code and invokes the SendResetPasswordEmailProcess.
    /// </summary>
    public sealed class UserForgotPasswordRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;
        private readonly ApplicationOptions _options;
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="auditUserApiService"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="options"></param>
        /// <param name="businessRuleService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        public UserForgotPasswordRule(
            ILoggerFactory loggerFactory,
            IUserAuditApiService auditUserApiService,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService,
            IOptions<ApplicationOptions> options,
            IBusinessRuleService businessRuleService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UserForgotPasswordRule>();
            _auditUserApiService = auditUserApiService;
            _userManagerService = userManagerApiService;
            _iPAddressService = iPAddressService;
            _options = options.Value;
            _businessRuleService = businessRuleService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserForgotPasswordProcess),
                typeof(UserForgotPasswordRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            return ExecuteRuleAsync(context).GetAwaiter().GetResult();
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
                var e = context.Object as UserForgotPasswordProcess;
                if (e == null)
                    return response;

                // AI: Find the user
                var respUser = await _userManagerService.FindByIdAsync(e.DomainObject.ToString());
                if (respUser.Error || respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                // AI: Generate the password reset token
                var user = e.DomainObject;
                var respCode = await _userManagerService.GeneratePasswordResetTokenAsync(respUser.Item.StorageKey);
                if (respCode.Error)
                {
                    response.CopyFrom(respCode);
                    return response;
                }

                // AI: Create the callback URL
                string baseUrl = _configuration.GetValue<string>(SecurityConstants.APPSETTING_SECURITY_CALLBACKURL);
                if (string.IsNullOrEmpty(baseUrl))
                    baseUrl = _options.Url;
                string callbackUrl = string.Format(
                        "{0}/ResetPassword?code={1}&userId={2}",
                        baseUrl, respCode.Item, e.DomainObject);

                // AI: Send the reset email process
                SendResetPasswordEmailProcess sendProcess = new SendResetPasswordEmailProcess(
                    respUser.Item, callbackUrl);
                var respSend = await _businessRuleService.ExecuteProcessAsync(sendProcess);

                // AI: Audit user
                await _auditUserApiService.CreateAsync(new UserAuditDto()
                {
                    AuditType = AuditType.FORGOT_PASSWORD_TEXT,
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