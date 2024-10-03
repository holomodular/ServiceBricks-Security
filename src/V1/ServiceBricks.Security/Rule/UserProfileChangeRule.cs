using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule updates a user profile.
    /// </summary>
    public sealed class UserProfileChangeRule : BusinessRule
    {
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IUserApiService _applicationUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="applicationUserApiService"></param>
        public UserProfileChangeRule(
            IUserAuditApiService auditUserApiService,
            IUserManagerService userManagerApiService,
            IHttpContextAccessor httpContextAccessor,
            IIpAddressService iPAddressService,
            IUserApiService applicationUserApiService)
        {
            _auditUserApiService = auditUserApiService;
            _userManagerService = userManagerApiService;
            _httpContextAccessor = httpContextAccessor;
            _iPAddressService = iPAddressService;
            _applicationUserApiService = applicationUserApiService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register this business rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserProfileChangeProcess),
                typeof(UserProfileChangeRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(UserProfileChangeProcess),
                typeof(UserProfileChangeRule));
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
            var e = context.Object as UserProfileChangeProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Find the user
            var respUser = _userManagerService.FindById(e.UserStorageKey.ToString());
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Change user properties here

            // AI: Update the user
            var respUpdate = _applicationUserApiService.Update(respUser.Item);
            if (respUpdate.Error)
            {
                response.CopyFrom(respUpdate);
                return response;
            }

            // AI: Audit user
            _auditUserApiService.Create(new UserAuditDto()
            {
                AuditType = AuditType.PROFILE_CHANGE_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = respUser.Item.StorageKey,
                IPAddress = _iPAddressService.GetIPAddress()
            });

            // AI: Tell the usermanager to use updated identity
            if (_httpContextAccessor != null &&
                _httpContextAccessor.HttpContext != null)
            {
                _userManagerService.RefreshSignIn(respUser.Item.StorageKey);
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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as UserProfileChangeProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Find the user
            var respUser = await _userManagerService.FindByIdAsync(e.UserStorageKey.ToString());
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Change user properties here

            // AI: Update the user
            var respUpdate = await _applicationUserApiService.UpdateAsync(respUser.Item);
            if (respUpdate.Error)
            {
                response.CopyFrom(respUpdate);
                return response;
            }

            // AI: Audit user
            await _auditUserApiService.CreateAsync(new UserAuditDto()
            {
                AuditType = AuditType.PROFILE_CHANGE_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = respUser.Item.StorageKey,
                IPAddress = _iPAddressService.GetIPAddress()
            });

            // AI: Tell the usermanager to use updated identity
            if (_httpContextAccessor != null &&
                _httpContextAccessor.HttpContext != null)
            {
                await _userManagerService.RefreshSignInAsync(respUser.Item.StorageKey);
            }

            return response;
        }
    }
}