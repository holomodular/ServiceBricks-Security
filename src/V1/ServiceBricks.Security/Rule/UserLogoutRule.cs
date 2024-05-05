using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user logs out.
    /// </summary>
    public partial class UserLogoutRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        public UserLogoutRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IUserManagerService userManagerApiService,
            IHttpContextAccessor httpContextAccessor,
            IIpAddressService iPAddressService
            )
        {
            _logger = loggerFactory.CreateLogger<UserLogoutRule>();
            _auditUserApiService = auditUserApiService;
            _userManagerService = userManagerApiService;
            _httpContextAccessor = httpContextAccessor;
            _iPAddressService = iPAddressService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserLogoutProcess),
                typeof(UserLogoutRule));
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
                var e = context.Object as UserLogoutProcess;
                if (e == null)
                    return response;

                // Logoff and replace with generic principal
                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
                {
                    await _userManagerService.SignOutAsync();
                    _httpContextAccessor.HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                }

                // Audit
                if (!string.IsNullOrEmpty(e.UserStorageKey))
                {
                    await _auditUserApiService.CreateAsync(new AuditUserDto()
                    {
                        AuditName = AuditType.LOGOUT,
                        UserAgent = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent,
                        UserStorageKey = e.UserStorageKey,
                        IPAddress = _iPAddressService.GetIPAddress()
                    });
                }
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