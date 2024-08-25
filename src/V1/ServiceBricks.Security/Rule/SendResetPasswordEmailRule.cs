using Microsoft.Extensions.Logging;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule sends an email to a user to reset their password.
    /// </summary>
    public sealed class SendResetPasswordEmailRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="serviceBus"></param>
        public SendResetPasswordEmailRule(
            ILoggerFactory loggerFactory,
            IServiceBus serviceBus)
        {
            _logger = loggerFactory.CreateLogger<SendResetPasswordEmailRule>();
            _serviceBus = serviceBus;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(SendResetPasswordEmailProcess),
                typeof(SendResetPasswordEmailRule));
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
                response.AddMessage(ResponseMessage.CreateError("Error sending email"));
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
                response.AddMessage(ResponseMessage.CreateError("Error sending email"));
            }
            return response;
        }

        private CreateApplicationEmailBroadcast GetEmailBroadcast(IBusinessRuleContext context)
        {
            // AI: Make sure the context object is the correct type
            var obj = context.Object as SendResetPasswordEmailProcess;
            if (obj == null)
                return null;

            // AI: Create Email
            var emailHtml = EMAIL_TEMPLATE_HTML.Replace("{0}", obj.CallbackUrl);
            var emailText = EMAIL_TEMPLATE_TEXT.Replace("{0}", obj.CallbackUrl);
            ApplicationEmailDto email = new ApplicationEmailDto()
            {
                ToAddress = obj.ApplicationUser.Email,
                Subject = "Password Reset",
                Body = emailText,
                BodyHtml = emailHtml,
                IsHtml = true
            };

            // AI: Create Email Broadcast
            return new CreateApplicationEmailBroadcast(email);
        }

        private const string EMAIL_TEMPLATE_TEXT = @"Change Your Password.
We have received a request to change your password.
Copy and paste the following link into a web browser to change your password.
{0}";

        private const string EMAIL_TEMPLATE_HTML = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
<meta name=""viewport"" content=""width=device-width"">
<title>Change Your Password.</title>
</head>
<body>
    <h1>We have received a request to change your password.</h1>
    <p>
        <a href=""{0}"">Click Here to change your password.</a>
    </p>
    <p>
        Or
    </p>
    <p>
        Copy and paste the following link into a web browser to change your password.
    </p>
    <p>
        {0}
    </p>
</body>
</html>
";
    }
}