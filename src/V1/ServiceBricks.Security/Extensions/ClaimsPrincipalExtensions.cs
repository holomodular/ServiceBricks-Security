using System.Linq;
using System.Security.Claims;

namespace ServiceBricks.Security
{
    /// <summary>
    /// ClaimsPrincipal extensions for the Security module.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
        }
    }
}