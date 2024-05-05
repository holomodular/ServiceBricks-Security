using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule updates a user profile.
    /// </summary>
    public partial class UserProfileChangeRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IApplicationUserApiService _applicationUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpAddressService _iPAddressService;

        public UserProfileChangeRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IUserManagerService userManagerApiService,
            IHttpContextAccessor httpContextAccessor,
            IIpAddressService iPAddressService,
            IApplicationUserApiService applicationUserApiService)
        {
            _logger = loggerFactory.CreateLogger<UserProfileChangeRule>();
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
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
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
                var e = context.Object as UserProfileChangeProcess;
                if (e == null)
                    return response;

                var respUser = await _userManagerService.FindByIdAsync(e.UserStorageKey.ToString());
                if (respUser.Error || respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                    return response;
                }

                // Change anything here

                var respUpdate = await _applicationUserApiService.UpdateAsync(respUser.Item);
                if (respUpdate.Error)
                {
                    response.CopyFrom(respUpdate);
                    return response;
                }

                // Audit
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.PROFILE_CHANGE,
                    UserAgent = _httpContextAccessor?.HttpContext?.Request?.Headers?.UserAgent,
                    UserStorageKey = respUser.Item.StorageKey,
                    IPAddress = _iPAddressService.GetIPAddress()
                });

                // Tell the authentication manager to use this new identity
                if (_httpContextAccessor != null &&
                    _httpContextAccessor.HttpContext != null)
                {
                    await _userManagerService.RefreshSignInAsync(respUser.Item.StorageKey);
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