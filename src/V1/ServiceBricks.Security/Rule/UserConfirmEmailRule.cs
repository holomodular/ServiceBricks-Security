using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule confirms a user email by using a code.
    /// </summary>
    public sealed class UserConfirmEmailRule : BusinessRule
    {
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="userManagerService"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="httpContextAccessor"></param>
        public UserConfirmEmailRule(
            IUserAuditApiService auditUserApiService,
            IUserManagerService userManagerService,
            IIpAddressService iPAddressService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _auditUserApiService = auditUserApiService;
            _userManagerService = userManagerService;
            _iPAddressService = iPAddressService;
            _httpContextAccessor = httpContextAccessor;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserConfirmEmailProcess),
                typeof(UserConfirmEmailRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(UserConfirmEmailProcess),
                typeof(UserConfirmEmailRule));
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
            var e = context.Object as UserConfirmEmailProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Find the user
            var respUser = _userManagerService.FindById(e.UserStorageKey);
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Confirm email with code
            var result = _userManagerService.ConfirmEmail(respUser.Item.StorageKey, e.Code);
            if (result.Error)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Audit user
            _auditUserApiService.Create(new UserAuditDto()
            {
                AuditType = AuditType.CONFIRM_EMAIL_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = respUser.Item.StorageKey,
                IPAddress = _iPAddressService.GetIPAddress()
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
            var e = context.Object as UserConfirmEmailProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Find the user
            var respUser = await _userManagerService.FindByIdAsync(e.UserStorageKey);
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Confirm email with code
            var result = await _userManagerService.ConfirmEmailAsync(respUser.Item.StorageKey, e.Code);
            if (result.Error)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Audit user
            await _auditUserApiService.CreateAsync(new UserAuditDto()
            {
                AuditType = AuditType.CONFIRM_EMAIL_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = respUser.Item.StorageKey,
                IPAddress = _iPAddressService.GetIPAddress()
            });

            return response;
        }
    }
}