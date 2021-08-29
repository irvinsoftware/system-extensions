using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Irvin.Extensions
{
    public class Url : ICloneable, ICloneable<Url>
    {
        public const string DefaultWebSubDomain = "www";

        private Url()
        {
        }

        public Url(string rawUrl)
        {
            if (IsAbsoluteUrl(rawUrl))
            {
                FillPropertiesFrom(new Uri(rawUrl));
            }
            else
            {
                PopulateBaseLocationPropertiesFrom(rawUrl);
            }
        }

        public Url(Uri uri)
        {
            FillPropertiesFrom(uri);
        }

        private void FillPropertiesFrom(Uri uri)
        {
            Protocol = GetProtocolFromScheme(uri.Scheme);
            PopulateBaseLocationPropertiesFrom(uri.Host);
            PortNumber = uri.Port;

            if (!string.IsNullOrEmpty(uri.PathAndQuery))
            {
                string[] subUriParts = uri.PathAndQuery.Split('?');
                PopulatePathDetailsFrom(subUriParts.FirstOrDefault());
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

        private void PopulateBaseLocationPropertiesFrom(string uriHost)
        {
            string[] hostParts = uriHost.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (hostParts.Length == 1)
            {
                DomainName = hostParts.First();
            }
            else if (hostParts.Length == 2)
            {
                DomainName = hostParts.First();
                TopLevelDomain = hostParts.Last();
            }
            else if (hostParts.Length == 3)
            {
                SubDomain = hostParts[0];
                DomainName = hostParts[1];
                TopLevelDomain = hostParts[2];
            }
            else
            {
                SubDomain = string.Join(".", hostParts.ToList().GetRange(0, hostParts.Length - 2));
                DomainName = hostParts[hostParts.Length - 2];
                TopLevelDomain = hostParts.Last();    
            }
        }

        public bool IsSecureConnection => Protocol == UrlProtocol.Https;
        public UrlProtocol Protocol { get; set; }
        public string ProtocolCode => GetProtocolCode(Protocol);
        public string Host => string.Join(".", new[] {SubDomain, DomainName, TopLevelDomain}.Where(x => x != null));
        public string SubDomain { get; set; }
        public string DomainName { get; private set; }
        public string TopLevelDomain { get; private set; }
        public int PortNumber { get; set; }
        public IReadOnlyList<string> PathFolders { get; private set; }

        public string Path
        {
            get => string.Join("/", PathFolders);
            set => PopulatePathDetailsFrom(value);
        }

        public string ResourceName { get; private set; }
        public string ResourceExtension { get; private set; }

        private void PopulatePathDetailsFrom(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
            {
                if (fullPath.StartsWith("/"))
                {
                    fullPath = fullPath.Substring(1);
                }

                string[] pathParts = fullPath.Split('/');
                string fileName = pathParts[pathParts.Length - 1];

                PathFolders = pathParts.ToList().GetRange(0, pathParts.Length - 1);

                SetResourceValuesFrom(fileName);
            }
        }
        
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

        public Url ResolveTo(string newPath)
        {
            Url newUrl = (Url) Clone();

            if (newPath.Contains("/"))
            {
                throw new NotImplementedException();
            }

            newUrl.SetResourceValuesFrom(newPath);

            return newUrl;
        }

        private void SetResourceValuesFrom(string fileName)
        {
            ResourceName = fileName;

            string[] resourceParts = ResourceName.Split('.');
            ResourceExtension = resourceParts.Length > 1 ? resourceParts[1] : null;
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
        
        public bool StartsWith(Url otherUrl)
        {
            //TODO: better implementation
            return this.ToString().StartsWith(otherUrl.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        Url ICloneable<Url>.Clone()
        {
            return (Url) Clone();
        }

        public object Clone()
        {
            Url source = this;
            return new Url
            {
                Protocol = source.Protocol,
                SubDomain = source.SubDomain,
                DomainName = source.DomainName,
                TopLevelDomain = source.TopLevelDomain,
                PortNumber = source.PortNumber,
                Path = source.Path
            };
        }

        public static int GetDefaultPortFor(UrlProtocol urlProtocol)
        {
            switch (urlProtocol)
            {
                case UrlProtocol.Https:
                    return 443;
                case UrlProtocol.Http:
                    return 80;
                default:
                    throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            string protocolCode = GetProtocolCode(Protocol);
            string portOutput = PortNumber != GetDefaultPortFor(Protocol) ? ":" + PortNumber : null;
            return $"{protocolCode}://{Host}{portOutput}{Path}/{ResourceName}";
        }

        public static string GetProtocolCode(UrlProtocol protocol)
        {
            switch (protocol)
            {
                case UrlProtocol.Http:
                    return "http";
                case UrlProtocol.Https:
                    return "https";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
