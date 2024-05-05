using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule changes a user password.
    /// </summary>
    public partial class UserPasswordChangeRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;

        public UserPasswordChangeRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService)
        {
            _logger = loggerFactory.CreateLogger<UserPasswordChangeRule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerApiService;
            _iPAddressService = iPAddressService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserPasswordChangeProcess),
                typeof(UserPasswordChangeRule));
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
                var e = context.Object as UserPasswordChangeProcess;
                if (e == null)
                    return response;

                // Logic
                var respUser = await _userManagerService.FindByIdAsync(e.UserStorageKey.ToString());
                if (respUser.Error || respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                    return response;
                }

                var result = await _userManagerService.ChangePasswordAsync(respUser.Item.StorageKey, e.OldPassword, e.NewPassword);
                if (result.Error)
                {
                    response.CopyFrom(result);
                    return response;
                }

                //Audit
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.PASSWORD_CHANGE,
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