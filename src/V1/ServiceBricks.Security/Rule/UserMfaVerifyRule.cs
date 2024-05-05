using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This a business rule happens when MFA is needed.
    /// </summary>
    public partial class UserMfaVerifyRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IIpAddressService _iPAddressService;
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public UserMfaVerifyRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManagerApiService,
            LinkGenerator linkGenerator,
            IIpAddressService iPAddressService,
            IBusinessRuleService businessRuleService,
            IServiceBus serviceBus)
        {
            _logger = loggerFactory.CreateLogger<UserMfaVerifyRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerApiService;
            _linkGenerator = linkGenerator;
            _iPAddressService = iPAddressService;
            _businessRuleService = businessRuleService;
            _serviceBus = serviceBus;
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
                var p = context.Object as UserMfaVerifyProcess;
                if (p == null)
                    return response;

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

                // Find the user
                var respUser = await _userManagerService.GetTwoFactorAuthenticationUserAsync();
                if (respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                // Audit
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.MFA_VERIFY,
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