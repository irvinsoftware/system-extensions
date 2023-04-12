using System;
using System.Collections;
using System.Linq;

namespace Irvin.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static int LastIndex(this ICollection list)
        {
            return list.Count - 1;
        }

        public static bool IsOneOf<T>(this T value, params T[] possibleValues)
        {
            return possibleValues.Any(x => x.Equals(value));
        }
    }
}