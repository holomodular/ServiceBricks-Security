using Microsoft.AspNetCore.Identity;

namespace ServiceBricks.Security
{
    /// <summary>
    /// IResponse extensions for the Security module.
    /// </summary>
    public static partial class ResponseExtensions
    {
        /// <summary>
        /// Copy messages from a response object to this instance.
        /// </summary>
        /// <param name="identityResult"></param>
        /// <param name="response"></param>
        public static IdentityResult GetIdentityResult(this IResponse response)
        {
            List<IdentityError> list = new List<IdentityError>();
            if (response != null && response.Messages != null)
            {
                foreach (var item in response.Messages)
                {
                    if (item.Severity == ResponseSeverity.ErrorSystemSensitive)
                        continue;
                    if (item.Severity != ResponseSeverity.Success)
                        list.Add(new IdentityError() { Description = item.Message });
                }
            }
            if (list.Count > 0)
                return IdentityResult.Failed(list.ToArray());
            return IdentityResult.Success;
        }

        /// <summary>
        /// Copy messages from a response object to this instance.
        /// </summary>
        /// <param name="identityResult"></param>
        /// <param name="response"></param>
        public static void CopyFrom(this IResponse response, IdentityResult result)
        {
            if (result == null || result.Errors == null)
                return;
            foreach (var item in result.Errors)
                response.AddMessage(ResponseMessage.CreateError(item.Description));
        }
    }
}