using System.Collections.Generic;
using Core.Exceptions;
using Core.Utils;

namespace Core.Collections
{
    public class Dictionary<TKey, TValue> : Object
    {
        private readonly List<TKey> _keys;
        private readonly List<TValue> _values;
        private List<KeyValuePair> KeyValuePairs { get; set; }

        public Dictionary()
        {
            KeyValuePairs = new List<KeyValuePair>();
            _keys = new List<TKey>();
            _values = new List<TValue>();
        }

        protected void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new KeyAlreadyExistsException(key);
            }

            KeyValuePairs.Add(new KeyValuePair(key, value));
            _keys.Add(key);
            _values.Add(value);
        }

        private Bool ContainsKey(TKey key)
        {
            return _keys.Contains(key);
        }

        public ReadOnlyList<TKey> Keys
        {
            get { return new ReadOnlyList<TKey>(_keys); }
        }

        public ReadOnlyList<TValue> Values
        {
            get { return new ReadOnlyList<TValue>(_values); }
        }

        public Bool TryGetValue(TKey key, out TValue value)
        {
            IEnumerable<TValue> enumerable = from keyValuePair in KeyValuePairs
                                             where MyEquals(keyValuePair.Key, key)
                                             select keyValuePair.Value;

            value = enumerable.FirstOrDefault();

            return !enumerable.IsEmpty();
        }

        public class KeyValuePair : ImmutableObject, IPair<TKey, TValue>
        {
            public KeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            TKey IPair<TKey, TValue>.First
            {
                get { return Key; }
            }

            public TKey Key { get; set; }

            TValue IPair<TKey, TValue>.Second
            {
                get { return Value; }
            }

            public TValue Value { get; set; }
        }
    }
}