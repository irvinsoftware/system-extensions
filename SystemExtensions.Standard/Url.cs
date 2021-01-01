using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Irvin.Extensions
{
    public class Url : ICloneable
    {
        public static int DefaultWebPort = 80;
        public const string DefaultWebSubDomain = "www";

        private Url()
        {
        }

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

        public bool IsSecureConnection => Protocol == UrlProtocol.Https;
        public UrlProtocol Protocol { get; private set; }
        public string Host => string.Join(".", new[] {SubDomain, DomainName, TopLevelDomain}.Where(x => x != null));
        public string SubDomain { get; private set; }
        public string DomainName { get; private set; }
        public string TopLevelDomain { get; private set; }
        public int PortNumber { get; private set; }
        public string Path { get; private set; }
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

        public Url ToSecure()
        {
            Url url = (Url) Clone();
            url.Protocol = UrlProtocol.Https;
            return url;
        }

        public bool Matches(Url url)
        {
            if (url == null)
            {
                return false;
            }

            Url thisUrl = this;
            Url otherUrl = url;

            return
                otherUrl.Protocol == thisUrl.Protocol &&
                otherUrl.PortNumber == thisUrl.PortNumber &&
                string.Equals(otherUrl.SubDomain, thisUrl.SubDomain, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(otherUrl.DomainName, thisUrl.DomainName, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(otherUrl.TopLevelDomain, thisUrl.TopLevelDomain, StringComparison.InvariantCultureIgnoreCase) && 
                string.Equals(otherUrl.Path, thisUrl.Path, StringComparison.InvariantCultureIgnoreCase);
        }

        public object Clone()
        {
            return new Url
            {
                Protocol = this.Protocol,
                SubDomain = this.SubDomain,
                DomainName = this.DomainName,
                TopLevelDomain = this.TopLevelDomain,
                PortNumber = this.PortNumber,
                Path = this.Path
            };
        }
    }
}
