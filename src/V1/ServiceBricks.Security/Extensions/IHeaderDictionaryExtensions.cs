using Microsoft.AspNetCore.Http;

namespace ServiceBricks.Security
{
    /// <summary>
    /// HeaderDictionary extensions for the Security module.
    /// </summary>
    public static partial class IHeaderDictionaryExtensions
    {
        /// <summary>
        /// Gets the user id from the claims principal.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetData(this IHeaderDictionary dictionary)
        {
            if (dictionary == null)
                return string.Empty;

            string output = string.Empty;
            foreach (var item in dictionary)
            {
                if (!string.IsNullOrEmpty(output))
                    output += Environment.NewLine;
                output += $"{item.Key}: {string.Join(",", item.Value.ToArray())}";
            }
            return output;
        }
    }
}