using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule happens when a user registers.
    /// </summary>
    public sealed class SendConfirmEmailRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="serviceBus"></param>
        public SendConfirmEmailRule(
            ILoggerFactory loggerFactory,
            IServiceBus serviceBus)
        {
            _logger = loggerFactory.CreateLogger<SendConfirmEmailRule>();
            _serviceBus = serviceBus;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(SendConfirmEmailProcess),
                typeof(SendConfirmEmailRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(SendConfirmEmailProcess),
                typeof(SendConfirmEmailRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                var broadcast = GetEmailBroadcast(context);
                if (broadcast != null)
                    _serviceBus.Send(broadcast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
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

            try
            {
                var broadcast = GetEmailBroadcast(context);
                if (broadcast != null)
                    await _serviceBus.SendAsync(broadcast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return response;
        }

        private CreateApplicationEmailBroadcast GetEmailBroadcast(IBusinessRuleContext context)
        {
            // AI: Make sure the context object is the correct type
            var obj = context.Object as SendConfirmEmailProcess;
            if (obj == null)
                return null;

            // AI: Create Email
            var emailHtml = EMAIL_TEMPLATE_HTML.Replace("{0}", obj.CallbackUrl);
            var emailText = EMAIL_TEMPLATE_TEXT.Replace("{0}", obj.CallbackUrl);
            ApplicationEmailDto email = new ApplicationEmailDto()
            {
                ToAddress = obj.ApplicationUser.Email,
                Subject = "Verify Your Email",
                Body = emailText,
                BodyHtml = emailHtml,
                IsHtml = true
            };

            // AI: Create Email Broadcast
            return new CreateApplicationEmailBroadcast(email);
        }

        private string EMAIL_TEMPLATE_TEXT = @"Verify Your Email.
Copy and paste the following link into a web browser to verify your email.
{0}";

        private string EMAIL_TEMPLATE_HTML = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
<meta name=""viewport"" content=""width=device-width"">
<title>Verify Your Email</title>
<body>
    <h1>Verify Your Email</h1>
    <p>
        <a href=""{0}"">Click Here to verify your email.</a>
    </p>
    <p>
        Or
    </p>
    <p>
        Copy and paste the following link into a web browser to verify your email.
    </p>
    <p>
        {0}
    </p>
</body>
</html>
";
    }
}