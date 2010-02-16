using System.Collections.Generic;
using MyFramework.Delegates;

namespace MyFramework.Collections
{
    public class Pool<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public Pool()
        {
        }

        public Pool(IEnumerable<KeyValuePair> pairs)
            : base(pairs)
        {
        }

        public Pool(IEnumerable<IPair<TKey, TValue>> pairs)
            : base(pairs)
        {
        }

        public Pool(params IPair<TKey, TValue>[] pairs)
            : base(pairs)
        {
        }

        public TValue GetOrAdd(TKey key, Delegate<TKey, TValue> method)
        {
            TValue value;

            if (TryGetValue(key, out value))
            {
                return value;
            }

            value = method.Invoke(key);

            this[key] = value;

            return value;
        }

        public TValue GetOrAdd(TKey key, Delegate<TKey, TValue>.InnerDelegate method)
        {
            return GetOrAdd(key, new Delegate<TKey, TValue>(method));
        }
    }
}