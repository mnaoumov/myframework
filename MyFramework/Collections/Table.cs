using System.Collections.Generic;
using MyFramework.Helpers;

namespace MyFramework.Collections
{
    public class Table<TKey, TValue> : Object
    {
        private readonly Dictionary<LinkedList<TKey>, TValue> innerDictionary =
            new Dictionary<LinkedList<TKey>, TValue>();

        public Table(params Row[] rows)
            : this(CollectionsHelper.Iterate(rows))
        {
        }

        public Table(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                innerDictionary[row.Keys] = row.Value;
            }
        }

        public TValue this[params TKey[] keys]
        {
            get { return innerDictionary[new LinkedList<TKey>(keys)]; }
        }

        public override String MyToString()
        {
            String toString = null;

            Bool first = Bool.True;

            foreach (Dictionary<LinkedList<TKey>, TValue>.KeyValuePair keyValuePair in innerDictionary)
            {
                if (first)
                {
                    first = Bool.False;
                }
                else
                {
                    toString += "\n";
                }

                foreach (TKey key in keyValuePair.Key)
                {
                    toString += key.ToString() + " ";
                }

                toString += "| " + keyValuePair.Value;
            }

            return toString;
        }

        #region Nested type: Row

        public class Row
        {
            private readonly TKey[] keys;
            private readonly TValue value;

            public Row(TValue value, params TKey[] keys)
            {
                this.value = value;
                this.keys = keys;
            }

            public LinkedList<TKey> Keys
            {
                get { return new LinkedList<TKey>(keys); }
            }

            public TValue Value
            {
                get { return value; }
            }
        }

        #endregion
    }
}