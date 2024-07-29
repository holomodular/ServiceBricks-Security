using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule resets a user password with a code from
    /// ApplicationUserForgotPasswordRule.
    /// </summary>
    public partial class UserPasswordResetRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManager;
        private readonly IIpAddressService _iPAddressService;
        private readonly IApplicationUserApiService _applicationUserApiService;

        public UserPasswordResetRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManager,
            IIpAddressService iPAddressService,
            IApplicationUserApiService applicationUserApiService)
        {
            _logger = loggerFactory.CreateLogger<UserPasswordResetRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _iPAddressService = iPAddressService;
            _applicationUserApiService = applicationUserApiService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserPasswordResetProcess),
                typeof(UserPasswordResetRule));
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
                var e = context.Object as UserPasswordResetProcess;
                if (e == null)
                    return response;

                // Logic
                var respUser = await _userManager.FindByEmailAsync(e.Email);
                if (respUser.Error || respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                // Fix for spaces
                string code = e.Code;
                if (!string.IsNullOrEmpty(code))
                    code = code.Replace(" ", "+");

                var result = await _userManager.ResetPasswordAsync(respUser.Item.StorageKey, e.Code, e.Password);
                response.CopyFrom(result);
                if (response.Error)
                    return response;

                // Code was available via email
                if (!respUser.Item.EmailConfirmed)
                {
                    var respU = await _applicationUserApiService.GetAsync(respUser.Item.StorageKey);
                    if (respU.Item != null)
                    {
                        respU.Item.EmailConfirmed = true;
                        await _applicationUserApiService.UpdateAsync(respU.Item);
                    }
                }

                // Audit
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.PASSWORD_RESET,
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