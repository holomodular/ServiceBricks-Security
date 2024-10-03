namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user registers as an admin.
    /// </summary>
    public sealed class UserRegisterAdminRule : BusinessRule
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IUserApiService _applicationUserApiService;
        private readonly IBusinessRuleService _businessRuleService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationUserApiService"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="businessRuleService"></param>
        public UserRegisterAdminRule(
            IUserApiService applicationUserApiService,
            IUserManagerService userManagerApiService,
            IBusinessRuleService businessRuleService)
        {
            _applicationUserApiService = applicationUserApiService;
            _businessRuleService = businessRuleService;
            _userManagerService = userManagerApiService;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserRegisterAdminProcess),
                typeof(UserRegisterAdminRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
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
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as UserRegisterAdminProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Call user register Process (no confirmation email)
            UserRegisterProcess registerProcess = new UserRegisterProcess(
                e.Email, e.Password, false, true);
            var respRegisterUser = _businessRuleService.ExecuteProcess(registerProcess);
            if (respRegisterUser.Error)
            {
                response.CopyFrom(respRegisterUser);
                return response;
            }

            // AI: Find the user by email
            var respUser = _userManagerService.FindByEmail(e.Email);
            if (respUser.Item != null)
            {
                // AI: Add the admin role
                var respAdminRole = _userManagerService.AddToRole(respUser.Item.StorageKey, ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME);
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
            var e = context.Object as UserRegisterAdminProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Call user register Process (no confirmation email)
            UserRegisterProcess registerProcess = new UserRegisterProcess(
                e.Email, e.Password, false, true);
            var respRegisterUser = await _businessRuleService.ExecuteProcessAsync(registerProcess);
            if (respRegisterUser.Error)
            {
                response.CopyFrom(respRegisterUser);
                return response;
            }

            // AI: Find the user by email
            var respUser = await _userManagerService.FindByEmailAsync(e.Email);
            if (respUser.Item != null)
            {
                // AI: Add the admin role
                var respAdminRole = await _userManagerService.AddToRoleAsync(respUser.Item.StorageKey, ServiceBricksConstants.SECURITY_ROLE_ADMIN_NAME);
            }

            return response;
        }
    }
}