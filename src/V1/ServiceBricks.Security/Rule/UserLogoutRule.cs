using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user logs out.
    /// </summary>
    public sealed class UserLogoutRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="auditUserApiService"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="iPAddressService"></param>
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
                // AI: Make sure the context object is the correct type
                var e = context.Object as UserLogoutProcess;
                if (e == null)
                    return response;

                // AI: If context not null, logoff and replace with generic principal
                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
                {
                    await _userManagerService.SignOutAsync();
                    _httpContextAccessor.HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                }

                // AI: Audit user
                if (!string.IsNullOrEmpty(e.UserStorageKey))
                {
                    await _auditUserApiService.CreateAsync(new AuditUserDto()
                    {
                        AuditName = AuditType.LOGOUT_TEXT,
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