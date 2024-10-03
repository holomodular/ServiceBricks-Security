namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule sends an email to a user to reset their password.
    /// </summary>
    public sealed class SendResetPasswordEmailRule : BusinessRule
    {
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBus"></param>
        public SendResetPasswordEmailRule(
            IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
            Priority = PRIORITY_NORMAL;
        }

        /// <summary>
        /// Register the business rule.
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(SendResetPasswordEmailProcess),
                typeof(SendResetPasswordEmailRule));
        }

        /// <summary>
        /// Unregister the rule
        /// </summary>
        /// <param name="registry"></param>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
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

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var obj = context.Object as SendResetPasswordEmailProcess;
            if (obj == null || obj.ApplicationUser == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            var broadcast = GetEmailBroadcast(obj);
            if (broadcast != null)
                _serviceBus.Send(broadcast);

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
            var obj = context.Object as SendResetPasswordEmailProcess;
            if (obj == null || obj.ApplicationUser == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            var broadcast = GetEmailBroadcast(obj);
            if (broadcast != null)
                await _serviceBus.SendAsync(broadcast);

            return response;
        }

        private CreateApplicationEmailBroadcast GetEmailBroadcast(SendResetPasswordEmailProcess obj)
        {
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