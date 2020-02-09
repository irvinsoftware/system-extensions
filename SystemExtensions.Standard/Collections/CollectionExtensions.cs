using System.Collections;

namespace Irvin.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static int LastIndex(this ICollection list)
        {
            return list.Count - 1;
        }
    }
}