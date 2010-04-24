using Core.Collections;

namespace Core.Factories
{
    public abstract class PooledFactory<TKey, TResult> : AbstractFactory<TKey, TResult>
    {
        private readonly Pool<TKey, TResult> _pool = new Pool<TKey, TResult>();

        public TResult GetOrCreate(TKey key)
        {
            return _pool.GetOrCreate(key, Create);
        }
    }
}