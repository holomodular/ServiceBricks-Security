﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ServiceBricks.Security
{
    /// <summary>
    /// This business rule sends an email to a user to reset their password.
    /// </summary>
    public partial class SendResetPasswordEmailRule : BusinessRule
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;

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
                var obj = context.Object as SendResetPasswordEmailProcess;
                if (obj == null)
                    return response;

                // Create Email Event
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
                var createEmailBroadcast = new CreateApplicationEmailBroadcast(email);
                _serviceBus.Send(createEmailBroadcast);
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
                var obj = context.Object as SendResetPasswordEmailProcess;
                if (obj == null)
                    return response;

                // Create Email Event
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
                var createEmailBroadcast = new CreateApplicationEmailBroadcast(email);
                await _serviceBus.SendAsync(createEmailBroadcast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError("Error sending email"));
            }
            return response;
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