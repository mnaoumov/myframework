using System.Collections;
using System.Collections.Generic;
using MyFramework.Helpers;

namespace MyFramework.Collections
{
    public class LinkedList<T> : Object, IEnumerable<T>
    {
        private ListItem start;
        private ListItem lastItem;

        public LinkedList()
        {
            start = new ListItem();
            lastItem = start;
        }

        public LinkedList(IEnumerable<T> enumerable)
            : this()
        {
            foreach (T item in enumerable)
            {
                Add(item);
            }
        }

        public LinkedList(params T[] items)
            : this(CollectionsHelper.Iterate(items))
        {
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            ListItem listItem = start.NextItem;

            while (listItem != null)
            {
                yield return listItem.Value;

                listItem = listItem.NextItem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(T item)
        {
            ListItem newLastItem = new ListItem(item);

            lastItem.NextItem = newLastItem;

            lastItem = newLastItem;
        }

        public static LinkedList<T> Join(LinkedList<T> left, LinkedList<T> right)
        {
            LinkedList<T> joined = left.Clone();

            joined.lastItem.NextItem = right.start.NextItem;

            return joined;
        }

        public LinkedList<T> Clone()
        {
            return new LinkedList<T>(this);
        }

        public override String MyToString()
        {
            return CollectionsHelper.MyToString(this);
        }

        public override Bool MyEquals(object obj)
        {
            return MyEquals(obj, new Delegates.Predicate<LinkedList<T>>(
                                     delegate(LinkedList<T> linkedList) { return CollectionsHelper.AreEqual(this, linkedList); }));
        }

        #region Nested type: ListItem

        private class ListItem : Object, IPair<T, ListItem>
        {
            private readonly IPair<T, ListItem> innerPair;

            public ListItem()
                : this(default(T))
            {
            }

            public ListItem(T value)
                : this(value, null)
            {
            }

            public ListItem(T value, ListItem nextItem)
            {
                innerPair = new Pair<T, ListItem>(value, nextItem);
            }

            public ListItem NextItem
            {
                get { return ((IPair<T, ListItem>)this).Second; }
                set { ((IPair<T, ListItem>)this).Second = value; }
            }

            public T Value
            {
                get { return ((IPair<T, ListItem>)this).First; }
            }

            T IPair<T, ListItem>.First
            {
                get { return innerPair.First; }
                set { innerPair.First = value; }
            }

            ListItem IPair<T, ListItem>.Second
            {
                get { return innerPair.Second; }
                set { innerPair.Second = value; }
            }
        }

        #endregion
    }

    public static class LinkedList
    {
        public static LinkedList<T> Join<T>(LinkedList<T> left, LinkedList<T> right)
        {
            return LinkedList<T>.Join(left, right);
        }
    }
}