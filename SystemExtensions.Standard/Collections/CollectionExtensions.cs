using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Irvin.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static T PopFirst<T>(this List<T> list)
        {
            if (list.Any())
            {
                T item = list.First();
                list.RemoveAt(0);
                return item;
            }

            return default;
        }

        public static List<T> PopRange<T>(this List<T> list, int startIndex, int count)
        {
            List<T> newList = new List<T>();

            while (newList.Count < count)
            {
                newList.Add(list[startIndex]);
                list.RemoveAt(startIndex);
            }
            
            return newList;
        }
        
        public static T PopLast<T>(this List<T> list)
        {
            if (list.Any())
            {
                T item = list.Last();
                list.RemoveAt(list.Count - 1);
                return item;
            }

            return default;
        }
        
        public static int LastIndex(this ICollection list)
        {
            return list.Count - 1;
        }
        
        public static bool IsOneOf<T>(this T value, params T[] possibleValues)
        {
            return possibleValues.Any(x => x.Equals(value));
        }

        public static bool OnlyOne<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 1;
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

        public static IEnumerable<TSource> UnionAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.Concat(second);
        }

        public static string Concatenate(this IEnumerable<string> strings)
        {
            return string.Join(string.Empty, strings);
        }

        public static List<List<T>> Split<T>(this IEnumerable<T> collection, T delimiter)
            where T : IComparable
        {
            return Split(collection, item => item.CompareTo(delimiter) == 0);
        }

        public static List<List<T>> Split<T>(this IEnumerable<T> collection, Func<T, bool> splitIdentifier)
        {
            List<List<T>> groups = new List<List<T>>();

            List<T> buffer = null;
            foreach (T item in collection)
            {
                if (splitIdentifier(item))
                {
                    groups.Add(buffer ?? new List<T>());
                    buffer = null;
                }
                else
                {
                    if (buffer == null) buffer = new List<T>(); 
                    buffer.Add(item);
                }
            }

            if (buffer != null && buffer.Any())
            {
                groups.Add(buffer);
            }
            
            return groups;
        }
    }
}