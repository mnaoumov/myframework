using Core.Delegates;

namespace Core.Collections
{
    public class Pool<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public TValue GetOrCreate(TKey key, Creator<TKey, TValue> creator)
        {
            TValue value;
            
            if (TryGetValue(key, out value))
            {
                return value;
            }

            value = creator(key);

            Add(key, value);

            return value;
        }
    }
}