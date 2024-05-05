using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This a business rule happens when MFA is needed.
    /// </summary>
    public partial class UserMFARule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IAuditUserApiService _auditUserApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IIpAddressService _iPAddressService;
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public UserMFARule(
            ILoggerFactory loggerFactory,
            IAuditUserApiService auditUserApiService,
            IHttpContextAccessor httpContextAccessor,
            IUserManagerService userManagerApiService,
            LinkGenerator linkGenerator,
            IIpAddressService iPAddressService,
            IBusinessRuleService businessRuleService,
            IServiceBus serviceBus)
        {
            _logger = loggerFactory.CreateLogger<UserMFARule>();
            _auditUserApiService = auditUserApiService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerApiService;
            _linkGenerator = linkGenerator;
            _iPAddressService = iPAddressService;
            _businessRuleService = businessRuleService;
            _serviceBus = serviceBus;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
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
                var e = context.Object as UserMfaProcess;
                if (e == null)
                    return response;

                // Create token
                var respUser = await _userManagerService.GetTwoFactorAuthenticationUserAsync();
                if (respUser.Error || respUser.Item == null)
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                    return response;
                }

                var respCode = await _userManagerService.GenerateTwoFactorTokenAsync(respUser.Item.StorageKey, e.SelectedProvider);
                if (respCode.Error || string.IsNullOrEmpty(respCode.Item))
                {
                    response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_SYSTEM));
                    return response;
                }

                // Send token
                switch (e.SelectedProvider.ToLower())
                {
                    case "phone":
                    case "text":
                    case "sms":

                        if (string.IsNullOrEmpty(respUser.Item.PhoneNumber))
                        {
                            _logger.LogError($"User missing phone number {respUser.Item.StorageKey}");
                            response.AddMessage(ResponseMessage.CreateError(""));
                            return response;
                        }

                        // Create SMS Event
                        ApplicationSmsDto sms = new ApplicationSmsDto()
                        {
                            Message = "Multi-Factor Authentication. Your login code is: {0}",
                            PhoneNumber = respUser.Item.PhoneNumber
                        };
                        var createSmsBroadcast = new CreateApplicationSmsBroadcast(sms);
                        await _serviceBus.SendAsync(createSmsBroadcast);
                        break;

                    case "email":
                    default:

                        // Create Email Event
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
                        var createEmailBroadcast = new CreateApplicationEmailBroadcast(email);
                        await _serviceBus.SendAsync(createEmailBroadcast);
                        break;
                }

                // Audit
                await _auditUserApiService.CreateAsync(new AuditUserDto()
                {
                    AuditName = AuditType.MFA_START,
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