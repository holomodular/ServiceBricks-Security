﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule confirms a user email by using a code.
    /// </summary>
    public partial class UserConfirmEmailRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserConfirmEmailRule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IUserManagerService userManagerService,
            IIpAddressService iPAddressService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = loggerFactory.CreateLogger<UserConfirmEmailRule>();
            _auditUserApiService = auditUserApiService;
            _userManagerService = userManagerService;
            _iPAddressService = iPAddressService;
            _httpContextAccessor = httpContextAccessor;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
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
                var e = context.Object as UserConfirmEmailProcess;
                if (e == null)
                    return response;

                // Logic
                var respUser = await _userManagerService.FindByIdAsync(e.UserStorageKey);
                if (respUser.Error || respUser.Item == null)
                {
                    _logger.LogWarning($"Warning confirming email. User not found: {e.UserStorageKey}");
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                    return response;
                }

                var result = await _userManagerService.ConfirmEmailAsync(respUser.Item.StorageKey, e.Code);
                if (result.Error)
                {
                    _logger.LogWarning($"Warning confirming email. Invalid code for user: {e.UserStorageKey} {e.Code}");
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                    return response;
                }

                // Audit
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.CONFIRM_EMAIL,
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