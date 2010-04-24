namespace Core.Factories
{
    public abstract class AbstractFactory<TKey, TResult> : Object
    {
        public abstract TResult Create(TKey key);
    }
}