namespace Core.Delegates
{
    public delegate TValue Creator<TKey, TValue>(TKey key);
}