using System;
using System.Text.RegularExpressions;

namespace Irvin.Extensions
{
    public class Url
    {
        public Url(Uri uri)
        {
            Host = uri.Host;

            if(!string.IsNullOrEmpty(uri.PathAndQuery))
            {
                string[] subUriParts = uri.PathAndQuery.Split('?');
                string path = subUriParts[0];

                if(!string.IsNullOrEmpty(path))
                {
                    string[] pathParts = path.Split('/');
                    ResourceName = pathParts[pathParts.Length - 1];

                    string[] resourceParts = ResourceName.Split('.');
                    ResourceExtension = resourceParts.Length > 1 ? resourceParts[1] : null;
                }                
            }            
        }

        public string Host { get; set; }
        public string ResourceName { get; set; }
        public string ResourceExtension { get; set; }

        public static bool IsAbsoluteUrl(string url)
        {
            return Regex.IsMatch(url, "^[a-z]+://.+", RegexOptions.IgnoreCase);
        }

        public static bool IsBookmark(string url)
        {
            return (url ?? string.Empty).Trim().StartsWith("#");
        }
    }
}
