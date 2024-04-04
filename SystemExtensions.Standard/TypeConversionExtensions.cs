using System;
using System.ComponentModel;
using System.Globalization;

namespace Irvin.Extensions
{
    public static class TypeConversionExtensions
    {
        public static object ConvertTo(this string rawValue, Type targetType, string dateTimeFormat = null, string fieldName = null)
        {
            try
            {
                if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                {
                    object defaultValue = targetType == typeof(DateTime) ? DateTime.MinValue : null;
                    return ParseDateTime(rawValue, dateTimeFormat, defaultValue);
                }

                if (targetType == typeof(decimal) || targetType == typeof(float))
                {
                    rawValue = rawValue.Replace("$", string.Empty).Replace(",", string.Empty);
                }

                if (targetType == typeof(object))
                {
                    return rawValue;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                return converter.ConvertFromString(rawValue);
            }
            catch (Exception conversionException)
            {
                string message = $"The value '{rawValue}' cannot be converted to a {targetType.Name}";
                if (!string.IsNullOrWhiteSpace(fieldName))
                {
                    message += $" (field '{fieldName}').";
                }
                throw new InvalidDataException(message, conversionException);
            }
        }

        private static object ParseDateTime(string rawValue, string format, object defaultValue)
        {
            rawValue = (rawValue ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(rawValue))
            {
                return defaultValue;
            }

            if (!string.IsNullOrEmpty(format))
            {
                return DateTime.ParseExact(rawValue, format, CultureInfo.InvariantCulture.DateTimeFormat);
            }

            return DateTime.Parse(rawValue);
        }
    }
}