namespace MyFramework.Wrappers
{
    public interface IWrapper<T>
    {
        T Value { get; set; }
    }
}