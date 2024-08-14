using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user requests another confirmation code.
    /// </summary>
    public sealed class UserResendConfirmationProcessRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
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
        public UserResendConfirmationProcessRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService,
            IOptions<ApplicationOptions> options,
            IBusinessRuleService businessRuleService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UserResendConfirmationProcessRule>();
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
                typeof(UserResendConfirmationProcess),
                typeof(UserResendConfirmationProcessRule));
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
                var e = context.Object as UserResendConfirmationProcess;
                if (e == null)
                    return response;

                // AI: Find the user
                var respUser = await _userManagerService.FindByIdAsync(e.UserStorageKey);
                if (respUser.Error || respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                // AI: Create confirmation code
                var respCode = await _userManagerService.GenerateEmailConfirmationTokenAsync(e.UserStorageKey);
                if (respCode.Error)
                {
                    response.CopyFrom(respCode);
                    return response;
                }

                // AI: Create callback URL
                string encodedConfirmCode = HttpUtility.UrlEncode(respCode.Item);
                string baseUrl = _configuration.GetValue<string>(SecurityConstants.APPSETTING_SECURITY_CALLBACKURL);
                if (string.IsNullOrEmpty(baseUrl))
                    baseUrl = _options.Url;
                string callbackUrl = string.Format(
                        "{0}/ConfirmEmail?code={1}&userId={2}",
                        baseUrl, encodedConfirmCode, e.UserStorageKey);

                // AI: Send confirm email process
                SendConfirmEmailProcess sendProcess = new SendConfirmEmailProcess(
                    respUser.Item, callbackUrl);
                var respSend = await _businessRuleService.ExecuteProcessAsync(sendProcess);

                // AI: Audit user
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.RESEND_CONFIRMATION_TEXT,
                    UserAgent = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent,
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