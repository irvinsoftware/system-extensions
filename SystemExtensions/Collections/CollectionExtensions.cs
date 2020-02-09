namespace System.Collections
{
    public static class CollectionExtensions
    {
        public static int LastIndex(this ICollection list)
        {
            return list.Count - 1;
        }
    }
}