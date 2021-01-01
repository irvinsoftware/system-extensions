using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Irvin.Extensions
{
    public class Url
    {
        public const string DefaultWebSubDomain = "www";

        public Url(Uri uri)
        {
            Protocol = GetProtocolFromScheme(uri.Scheme);

            string[] hostParts = uri.Host.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (hostParts.Length == 1)
            {
                DomainName = hostParts.First();
            }
            if (hostParts.Length == 2)
            {
                DomainName = hostParts.First();
                TopLevelDomain = hostParts.Last();
            }
            if (hostParts.Length == 3)
            {
                SubDomain = hostParts[0];
                DomainName = hostParts[1];
                TopLevelDomain = hostParts[2];
            }

            PortNumber = uri.Port;

            if(!string.IsNullOrEmpty(uri.PathAndQuery))
            {
                string[] subUriParts = uri.PathAndQuery.Split('?');
                Path = subUriParts[0];

                if(!string.IsNullOrEmpty(Path))
                {
                    string[] pathParts = Path.Split('/');
                    ResourceName = pathParts[pathParts.Length - 1];

                    string[] resourceParts = ResourceName.Split('.');
                    ResourceExtension = resourceParts.Length > 1 ? resourceParts[1] : null;
                }                
            }            
        }

        private UrlProtocol GetProtocolFromScheme(string uriScheme)
        {
            switch (uriScheme.ToLower().Trim())
            {
                case "http":
                    return UrlProtocol.Http;
                case "https":
                    return UrlProtocol.Https;
                default:
                    throw new NotSupportedException();
            }
        }

        public UrlProtocol Protocol { get; }
        public string Host => string.Join(".", new[] {SubDomain, DomainName, TopLevelDomain}.Where(x => x != null));
        public string SubDomain { get; }
        public string DomainName { get; }
        public string TopLevelDomain { get; }
        public int PortNumber { get; }
        public string Path { get; }
        public string ResourceName { get; }
        public string ResourceExtension { get; }

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
