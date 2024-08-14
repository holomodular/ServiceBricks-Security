using System.Security.Claims;

namespace ServiceBricks.Security
{
    /// <summary>
    /// ClaimsPrincipal extensions for the Security module.
    /// </summary>
    public static partial class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the user id from the claims principal.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetId(this ClaimsPrincipal user)
        {
            return user?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
        }
    }
}