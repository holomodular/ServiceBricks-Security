using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when an invalid password event is raised.
    /// </summary>
    public sealed class UserInvalidPasswordRule : BusinessRule
    {
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="iPAddressService"></param>
        public UserInvalidPasswordRule(
            IUserAuditApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IIpAddressService iPAddressService
            )
        {
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _iPAddressService = iPAddressService;
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserInvalidPasswordProcess),
                typeof(UserInvalidPasswordRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(UserInvalidPasswordProcess),
                typeof(UserInvalidPasswordRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var uipp = context.Object as UserInvalidPasswordProcess;
            if (uipp == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: If user not found, don't log anything to save space
            if (string.IsNullOrEmpty(uipp.UserStorageKey))
                return response;

            // AI: Audit found user
            _auditUserApiService.Create(new UserAuditDto()
            {
                AuditType = AuditType.INVALID_PASSWORD_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = uipp.UserStorageKey,
                IPAddress = _iPAddressService.GetIPAddress(),
            });

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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var uipp = context.Object as UserInvalidPasswordProcess;
            if (uipp == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: If user not found, don't log anything
            if (string.IsNullOrEmpty(uipp.UserStorageKey))
                return response;

            // AI: Audit found user
            await _auditUserApiService.CreateAsync(new UserAuditDto()
            {
                AuditType = AuditType.INVALID_PASSWORD_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = uipp.UserStorageKey,
                IPAddress = _iPAddressService.GetIPAddress(),
            });

            return response;
        }
    }
}