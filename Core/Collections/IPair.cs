namespace Core.Collections
{
    public interface IPair<TFirst, TSecond>
    {
        TFirst First { get; }
        TSecond Second { get; }
    }
}