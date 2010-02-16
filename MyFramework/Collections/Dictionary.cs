using System.Collections;
using System.Collections.Generic;
using MyFramework.Delegates;
using MyFramework.Helpers;

namespace MyFramework.Collections
{
    public class Dictionary<TKey, TValue> : Object, IEnumerable<Dictionary<TKey, TValue>.KeyValuePair>
    {
        private readonly LinkedList<KeyValuePair> pairs = new LinkedList<KeyValuePair>();
        private KeyValuePair cachedPair;

        public Dictionary()
            : this(CollectionsHelper.GetEmptyEnumerable<IPair<TKey, TValue>>())
        {
        }

        public Dictionary(IEnumerable<IPair<TKey, TValue>> pairs)
            : this(KeyValuePair.PrepareKeyValuePairs(pairs))
        {
        }

        public Dictionary(IEnumerable<KeyValuePair> pairs)
        {
            this.pairs = new LinkedList<KeyValuePair>(pairs);
        }


        public Dictionary(params IPair<TKey, TValue>[] pairs)
            : this(CollectionsHelper.Iterate(pairs))
        {
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;

                if (TryGetValue(key, out value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }
            set
            {
                foreach (KeyValuePair keyValuePair in pairs)
                {
                    if (ObjectsHelper.AreEqual(keyValuePair.Key, key))
                    {
                        keyValuePair.Value = value;
                        return;
                    }
                }

                pairs.Add(new KeyValuePair(key, value));
            }
        }

        #region IEnumerable<Dictionary<TKey,TValue>.KeyValuePair> Members

        public IEnumerator<KeyValuePair> GetEnumerator()
        {
            return pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public Bool TryGetValue(TKey key, out TValue value)
        {
            if (cachedPair != null && ObjectsHelper.AreEqual(key, cachedPair.Key))
            {
                value = cachedPair.Value;

                return Bool.True;
            }

            foreach (KeyValuePair keyValuePair in pairs)
            {
                if (ObjectsHelper.AreEqual(key, keyValuePair.Key))
                {
                    cachedPair = keyValuePair;

                    value = cachedPair.Value;

                    return Bool.True;
                }
            }

            value = default(TValue);

            return Bool.False;
        }

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new System.ArgumentException();
            }

            pairs.Add(new KeyValuePair(key, value));
        }

        private Bool ContainsKey(TKey key)
        {
            return CollectionsHelper.ForOne(this,
                                            new Predicate<KeyValuePair>(delegate(KeyValuePair pair) { return CheckForKey(pair, key); }));
        }

        private Bool CheckForKey(KeyValuePair pair, TKey key)
        {
            Bool containsKey = ObjectsHelper.AreEqual(pair.Key, key);

            if (containsKey)
            {
                cachedPair = pair;
            }

            return containsKey;
        }

        public override String MyToString()
        {
            return pairs.MyToString();
        }

        public override Bool MyEquals(object obj)
        {
            return MyEquals(obj, new Predicate<Dictionary<TKey, TValue>>(
                                     delegate(Dictionary<TKey, TValue> dictionary) { return CollectionsHelper.AreEqual(this, dictionary); }));
        }

        public Dictionary<TKey, TValue> Clone()
        {
            return new Dictionary<TKey, TValue>(this);
        }

        #region Nested type: KeyValuePair

        public class KeyValuePair : Object, IPair<TKey, TValue>
        {
            private readonly IPair<TKey, TValue> innerPair;

            public KeyValuePair(TKey key, TValue value)
                : this(new Pair<TKey, TValue>(key, value))
            {
            }

            public KeyValuePair(IPair<TKey, TValue> pair)
            {
                innerPair = pair;
            }

            public TKey Key
            {
                get { return ((IPair<TKey, TValue>)this).First; }
            }

            public TValue Value
            {
                get { return innerPair.Second; }
                set { innerPair.Second = value; }
            }

            #region IPair<TKey,TValue> Members

            TKey IPair<TKey, TValue>.First
            {
                get { return innerPair.First; }
                set { innerPair.First = value; }
            }

            TValue IPair<TKey, TValue>.Second
            {
                get { return innerPair.Second; }
                set { innerPair.Second = value; }
            }

            #endregion

            public override String MyToString()
            {
                return String.Format("({A}: {B})", new String.ParametersPair("A", Key),
                                     new String.ParametersPair("B", Value));
            }

            public override Bool MyEquals(object obj)
            {
                return MyEquals(obj, new Predicate<KeyValuePair>(
                                         delegate(KeyValuePair myKeyValuePair) { return innerPair == myKeyValuePair.innerPair; }));
            }

            public static IEnumerable<KeyValuePair> PrepareKeyValuePairs(IEnumerable<IPair<TKey, TValue>> pairs)
            {
                return CollectionsHelper.Transform(pairs,
                                                   new Transformer<IPair<TKey, TValue>, KeyValuePair>(
                                                       delegate(IPair<TKey, TValue> argument) { return new KeyValuePair(argument); }));
            }
        }

        #endregion
    }
}