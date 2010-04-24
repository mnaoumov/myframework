using System.Collections;
using System.Collections.Generic;
using Core.Utils;

namespace Core.Collections
{
    public class List<T> : Object, IReadList<T>
    {

        public override String MyToString()
        {
            return this.EnumerableToString();
        }

        public Bool UseSequentialEquals { get; set; }
        public List(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        private ItemNextPair _head;
        private ItemNextPair _tail;

        public List()
            : this(EnumerableUtils.Empty<T>())
        {
        }

        public void Add(T item)
        {
            var newTail = new ItemNextPair(item);

            if (IsEmpty())
            {
                _head = _tail = newTail;
            }
            else
            {
                _tail.Next = newTail;
                _tail = newTail;
            }
        }

        public override Bool MyEquals(object obj)
        {
            return MyEquals(obj as List<T>);
        }

        public Bool MyEquals(List<T> list)
        {
            if (list == null)
            {
                return Bool.False;
            }

            if (UseSequentialEquals)
            {
                return this.SequentialEquals(list);
            }

            return EqualsByReference(this, list);
        }

        public bool IsEmpty()
        {
            return _head == null;
        }

        public Bool Contains(T item)
        {
            return this.Contains<T>(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            ItemNextPair current = _head;

            while (current != null)
            {
                yield return current.Item;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ItemNextPair : ImmutableObject, IPair<T, ItemNextPair>
        {
            public ItemNextPair(T item, ItemNextPair next)
            {
                Item = item;
                Next = next;
            }

            public ItemNextPair()
                : this(default(T))
            {
            }

            public ItemNextPair(T item)
                : this(item, null)
            {
            }

            T IPair<T, ItemNextPair>.First
            {
                get { return Item; }
            }

            public T Item { get; set; }

            ItemNextPair IPair<T, ItemNextPair>.Second
            {
                get { return Next; }
            }

            public ItemNextPair Next { get; set; }
        }
    }
}