using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
        
        public static List<object> ToDynamicList<TModel>(this List<TModel> list)
        {
            List<object> downcastList = new List<object>();
            downcastList.AddRange(list.Cast<object>());
            return downcastList;
        }

        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken = default)
        {
            List<T> listAsync = new List<T>();
            await foreach (T item in enumerable.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                listAsync.Add(item);
            }
            return listAsync;
        }
    }
}