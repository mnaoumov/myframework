namespace MyFramework.Factories
{
    public interface IFactory<TProduct, TKey>
    {
        TProduct Create(TKey key);
    }
}