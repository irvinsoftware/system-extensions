using System.Collections;
using System.Collections.Generic;

namespace Irvin.Extensions.Collections;

public class GranularList<T>
{
    public GranularList()
    {
        FullContent = new List<T>();
        Reader = new PrivateReader(FullContent);
        Appender = new ListAppender(this);
    }

    private List<T> FullContent { get; }

    public IReadOnlyList<T> Reader { get; }
    
    private class PrivateReader : IReadOnlyList<T>
    {
        public PrivateReader(IList<T> source)
        {
            Source = source;
        }

        private IList<T> Source { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index] => Source[index];

        public int Count => Source.Count;
    }
    
    public ListAppender Appender { get; }

    public class ListAppender
    {
        internal ListAppender(GranularList<T> parent)
        {
            Parent = parent;
        }

        private GranularList<T> Parent { get; }

        public void Push(T item)
        {
            Parent.FullContent.Add(item);
        }

        public void Push(IEnumerable<T> items)
        {
            Parent.FullContent.AddRange(items);
        }
    }

    public List<T> Entirety => FullContent;
}