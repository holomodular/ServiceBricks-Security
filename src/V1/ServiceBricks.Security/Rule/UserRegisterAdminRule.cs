using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user registers as an admin.
    /// </summary>
    public partial class UserRegisterAdminRule : BusinessRule
    {
        private ILogger _logger;
        private readonly IUserManagerService _userManagerService;
        private readonly IApplicationUserApiService _applicationUserApiService;
        private readonly IBusinessRuleService _businessRuleService;

        public UserRegisterAdminRule(
            ILoggerFactory loggerFactory,
            IApplicationUserApiService applicationUserApiService,
            IUserManagerService userManagerApiService,
            IBusinessRuleService businessRuleService)
        {
            _logger = loggerFactory.CreateLogger<UserRegisterAdminRule>();
            _applicationUserApiService = applicationUserApiService;
            _businessRuleService = businessRuleService;
            _userManagerService = userManagerApiService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(UserRegisterAdminProcess),
                typeof(UserRegisterAdminRule));
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
                var e = context.Object as UserRegisterAdminProcess;
                if (e == null || response.Error)
                    return response;

                // Register Process
                UserRegisterProcess registerProcess = new UserRegisterProcess(
                    e.Email, e.Password, false, true);
                var respRegisterUser = await _businessRuleService.ExecuteProcessAsync(registerProcess);
                if (respRegisterUser.Error)
                {
                    response.CopyFrom(respRegisterUser);
                    return response;
                }

                var respUser = await _userManagerService.FindByEmailAsync(e.Email);
                if (respUser.Item != null)
                {
                    // Add admin role
                    var respAdminRole = await _userManagerService.AddToRoleAsync(respUser.Item.StorageKey, SecurityConstants.ROLE_ADMIN_NAME);
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