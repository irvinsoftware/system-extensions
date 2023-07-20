using System;
using System.Collections.Generic;
using System.Reflection;

//for backwards compatability, do not move namespace (yet)
namespace Irvin.Extensions
{
	public static class ReflectionExtensions
	{
		public const string PROPERTY_GET_NAME_PREFIX = "get_";
		public const string PROPERTY_SET_NAME_PREFIX = "set_";

		public static TAttribute GetAttributeInstance<TAttribute>(this Type type, bool inherit = false) where TAttribute : Attribute
		{
			return GetAttribute<TAttribute>(type, inherit);
		}

		public static TAttribute GetAttributeInstance<TAttribute>(this ParameterInfo parameterInfo, bool inherit = false) where TAttribute : Attribute
		{
			return GetAttribute<TAttribute>(parameterInfo, inherit);
		}

		private static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider codeElement, bool inherit = false) where TAttribute : Attribute
		{
			object[] attributes = codeElement.GetCustomAttributes(typeof (TAttribute), inherit);
			return attributes.Length != 0 ? (TAttribute) attributes[0] : null;
		}
		
		public static TAttribute GetAttribute<TAttribute>(this MemberInfo codeElement, bool inherit = false) where TAttribute : Attribute
		{
			object[] attributes = codeElement.GetCustomAttributes(typeof (TAttribute), inherit);
			return attributes.Length != 0 ? (TAttribute) attributes[0] : null;
		}

		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive || typeof(decimal) == type || typeof(string) == type || typeof(DateTime) == type;
		}

		public static bool IsGenericList(this Type type)
		{
		    return type.IsTypeOf(typeof(List<>)) || type.IsTypeOf(typeof(IEnumerable<>));
		}

	    public static bool IsTypeOf(this Type x, Type y)
	    {
	        if (x.IsGenericType)
	        {
	            Type genericTypeDefinition = x.GetGenericTypeDefinition();
	            return genericTypeDefinition == y;
	        }

	        return false;
	    }

	    public static bool IsProperty(this MethodInfo method)
        {
            return IsPropertyGet(method) || IsPropertySet(method);
        }

        public static bool IsPropertyGet(this MethodInfo method)
        {
            return method.Name.StartsWith(PROPERTY_GET_NAME_PREFIX, StringComparison.InvariantCultureIgnoreCase) &&
                   method.IsSpecialName;
        }

        public static bool IsPropertySet(this MethodInfo method)
        {
            return method.Name.StartsWith(PROPERTY_SET_NAME_PREFIX, StringComparison.InvariantCultureIgnoreCase) &&
                   method.IsSpecialName;
        }

        public static string GetPropertyName(this MethodInfo method)
        {
            if (method.Name.StartsWith(PROPERTY_GET_NAME_PREFIX))
            {
                return method.Name.Substring(PROPERTY_GET_NAME_PREFIX.Length);
            }

            if (method.Name.StartsWith(PROPERTY_SET_NAME_PREFIX))
            {
                return method.Name.Substring(PROPERTY_SET_NAME_PREFIX.Length);
            }

            return null;
        }
    }
}