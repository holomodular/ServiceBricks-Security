﻿using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This a business rule happens when MFA is needed.
    /// </summary>
    public sealed class UserMFARule : BusinessRule
    {
        private readonly IUserAuditApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly IIpAddressService _iPAddressService;
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="auditUserApiService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userManagerApiService"></param>
        /// <param name="iPAddressService"></param>
        /// <param name="serviceBus"></param>
        public UserMFARule(
            IUserAuditApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManagerApiService,
            IIpAddressService iPAddressService,
            IServiceBus serviceBus)
        {
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerApiService;
            _iPAddressService = iPAddressService;
            _serviceBus = serviceBus;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(UserMfaProcess),
                typeof(UserMFARule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(UserMfaProcess),
                typeof(UserMFARule));
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
            var e = context.Object as UserMfaProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Get the user
            var respUser = _userManagerService.GetTwoFactorAuthenticationUser();
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Generate the token
            var respCode = _userManagerService.GenerateTwoFactorToken(respUser.Item.StorageKey, e.SelectedProvider);
            if (respCode.Error || string.IsNullOrEmpty(respCode.Item))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Send token
            switch (e.SelectedProvider.ToLower())
            {
                case "phone":
                case "text":
                case "sms":

                    // AI: Send via sms
                    if (string.IsNullOrEmpty(respUser.Item.PhoneNumber))
                    {
                        response.AddMessage(ResponseMessage.CreateError("PhoneNumber Missing"));
                        return response;
                    }
                    ApplicationSmsDto sms = new ApplicationSmsDto()
                    {
                        Message = "Multi-Factor Authentication. Your login code is: {0}",
                        PhoneNumber = respUser.Item.PhoneNumber
                    };

                    // AI: Create SMS broadcast
                    var createSmsBroadcast = new CreateApplicationSmsBroadcast(sms);

                    // AI: Send to servicebus
                    _serviceBus.Send(createSmsBroadcast);

                    break;

                case "email":
                default:

                    // AI: Send via email
                    var emailHtml = EMAIL_TEMPLATE_HTML.Replace("{0}", respCode.Item);
                    var emailText = EMAIL_TEMPLATE_TEXT.Replace("{0}", respCode.Item);
                    ApplicationEmailDto email = new ApplicationEmailDto()
                    {
                        ToAddress = respUser.Item.Email,
                        Subject = "Multi-Factor Authentication Login Request",
                        Body = emailText,
                        BodyHtml = emailHtml,
                        IsHtml = true
                    };

                    // AI: Create email broadcast
                    var createEmailBroadcast = new CreateApplicationEmailBroadcast(email);

                    // AI: Send to servicebus
                    _serviceBus.Send(createEmailBroadcast);

                    break;
            }

            // AI: Audit user
            _auditUserApiService.Create(new UserAuditDto()
            {
                AuditType = AuditType.MFA_START_TEXT,
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
            var e = context.Object as UserMfaProcess;
            if (e == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING));
                return response;
            }

            // AI: Get the user
            var respUser = await _userManagerService.GetTwoFactorAuthenticationUserAsync();
            if (respUser.Error || respUser.Item == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Generate the token
            var respCode = await _userManagerService.GenerateTwoFactorTokenAsync(respUser.Item.StorageKey, e.SelectedProvider);
            if (respCode.Error || string.IsNullOrEmpty(respCode.Item))
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                return response;
            }

            // AI: Send token
            switch (e.SelectedProvider.ToLower())
            {
                case "phone":
                case "text":
                case "sms":

                    // AI: Send via sms
                    if (string.IsNullOrEmpty(respUser.Item.PhoneNumber))
                    {
                        response.AddMessage(ResponseMessage.CreateError("PhoneNumber Missing"));
                        return response;
                    }
                    ApplicationSmsDto sms = new ApplicationSmsDto()
                    {
                        Message = "Multi-Factor Authentication. Your login code is: {0}",
                        PhoneNumber = respUser.Item.PhoneNumber
                    };

                    // AI: Create SMS broadcast
                    var createSmsBroadcast = new CreateApplicationSmsBroadcast(sms);

                    // AI: Send to servicebus
                    await _serviceBus.SendAsync(createSmsBroadcast);

                    break;

                case "email":
                default:

                    // AI: Send via email
                    var emailHtml = EMAIL_TEMPLATE_HTML.Replace("{0}", respCode.Item);
                    var emailText = EMAIL_TEMPLATE_TEXT.Replace("{0}", respCode.Item);
                    ApplicationEmailDto email = new ApplicationEmailDto()
                    {
                        ToAddress = respUser.Item.Email,
                        Subject = "Multi-Factor Authentication Login Request",
                        Body = emailText,
                        BodyHtml = emailHtml,
                        IsHtml = true
                    };

                    // AI: Create email broadcast
                    var createEmailBroadcast = new CreateApplicationEmailBroadcast(email);

                    // AI: Send to servicebus
                    await _serviceBus.SendAsync(createEmailBroadcast);

                    break;
            }

            // AI: Audit user
            await _auditUserApiService.CreateAsync(new UserAuditDto()
            {
                AuditType = AuditType.MFA_START_TEXT,
                RequestHeaders = _httpContextAccessor?.HttpContext?.Request?.Headers?.GetData(),
                UserStorageKey = respUser.Item.StorageKey,
                IPAddress = _iPAddressService.GetIPAddress()
            });

            return response;
        }

        private string EMAIL_TEMPLATE_TEXT = @"Multi-Factor Authentication.
Your login code is: {0}";

        private string EMAIL_TEMPLATE_HTML = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
<meta name=""viewport"" content=""width=device-width"">
<title>Multi-Factor Authentication.</title>
</head>
<body>
    <h1>Multi-Factor Authentication.</h1>
    <p>
        Your login code is: {0}
    </p>
</body>
</html>
";
    }
}