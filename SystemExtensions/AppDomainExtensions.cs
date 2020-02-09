using System;
using System.Reflection;

namespace Irvin.Extensions
{
    public static class AppDomainExtensions
    {
        public static string GetApplicationName(this AppDomain appDomain)
        {
            string name = appDomain.FriendlyName;

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                object[] matches = entryAssembly.GetCustomAttributes(typeof (AssemblyProductAttribute), false);
                if (matches.Length == 1)
                {
                    AssemblyProductAttribute productNameAttribute = matches[0] as AssemblyProductAttribute;
                    if (productNameAttribute != null)
                    {
                        name = productNameAttribute.Product;
                    }
                }
            }

            return name;
        }

        public static Version GetVersion(this AppDomain appDomain)
        {
            Version versionInfo = null;

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                versionInfo = entryAssembly.GetName().Version;
            }

            return versionInfo;
        }
    }
}