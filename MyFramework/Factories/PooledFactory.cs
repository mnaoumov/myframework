using MyFramework.Collections;
using MyFramework.Delegates;

namespace MyFramework.Factories
{
    public abstract class PooledFactory<TProduct, TKey> : Object, IFactory<TProduct, TKey>
    {
        private static readonly Pool<TKey, TProduct> pool = new Pool<TKey, TProduct>();

        public TProduct Create(TKey key)
        {
            return Create(key, new Creator<TKey,TProduct>(CreateNew));
        }

        public TProduct Create(TKey key,Creator<TKey,TProduct> creator)
        {
            return pool.GetOrAdd(key, new Delegate<TKey, TProduct>(creator.Create));
        }

        protected abstract TProduct CreateNew(TKey key);
    }
}