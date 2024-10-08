﻿using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule resets a user password with a code from
    /// ApplicationUserForgotPasswordRule.
    /// </summary>
    public sealed class UserPasswordResetRule : BusinessRule
    {
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManager;
        private readonly IIpAddressService _iPAddressService;
        private readonly IUserApiService _applicationUserApiService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userManager"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="applicationUserApiService"></param>
        public UserPasswordResetRule(
            IUserAuditApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManager,
            IIpAddressService iPAddressService,
            IUserApiService applicationUserApiService)
        {
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
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserPasswordResetProcess),
                typeof(UserPasswordResetRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
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
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as UserPasswordResetProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Find user by email
            var respUser = _userManager.FindByEmail(e.Email);
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // AI: Fix the code for spaces (encoding issue)
            string code = e.Code;
            if (!string.IsNullOrEmpty(code))
                code = code.Replace(" ", "+");

            // AI: Reset the password
            var result = _userManager.ResetPassword(respUser.Item.StorageKey, e.Code, e.Password);
            response.CopyFrom(result);
            if (response.Error)
                return response;

            // AI: Reset email confirmed (since they received the email)
            if (!respUser.Item.EmailConfirmed)
            {
                var respU = _applicationUserApiService.Get(respUser.Item.StorageKey);
                if (respU.Item != null)
                {
                    // AI: Update the user email confirmed
                    respU.Item.EmailConfirmed = true;
                    _applicationUserApiService.Update(respU.Item);
                }
            }

            // AI: Audit user
            _auditUserApiService.Create(new UserAuditDto()
            {
                AuditType = AuditType.PASSWORD_RESET_TEXT,
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
            var e = context.Object as UserPasswordResetProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Find user by email
            var respUser = await _userManager.FindByEmailAsync(e.Email);
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_ITEM_NOT_FOUND));
                return response;
            }

            // AI: Fix the code for spaces (encoding issue)
            string code = e.Code;
            if (!string.IsNullOrEmpty(code))
                code = code.Replace(" ", "+");

            // AI: Reset the password
            var result = await _userManager.ResetPasswordAsync(respUser.Item.StorageKey, e.Code, e.Password);
            response.CopyFrom(result);
            if (response.Error)
                return response;

            // AI: Reset email confirmed (since they received the email)
            if (!respUser.Item.EmailConfirmed)
            {
                var respU = await _applicationUserApiService.GetAsync(respUser.Item.StorageKey);
                if (respU.Item != null)
                {
                    // AI: Update the user email confirmed
                    respU.Item.EmailConfirmed = true;
                    await _applicationUserApiService.UpdateAsync(respU.Item);
                }
            }

            // AI: Audit user
            await _auditUserApiService.CreateAsync(new UserAuditDto()
            {
                AuditType = AuditType.PASSWORD_RESET_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = respUser.Item.StorageKey,
                IPAddress = _iPAddressService.GetIPAddress()
            });

            return response;
        }
    }
}