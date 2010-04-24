using System.Collections;
using System.Collections.Generic;

namespace Core.Collections
{
    public class ReadOnlyList<T> : Object, IReadList<T>
    {
        public ReadOnlyList(IEnumerable<T> items)
        {
            Items = new List<T>(items);
        }

        private List<T> Items { get; set; }

        public Bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}