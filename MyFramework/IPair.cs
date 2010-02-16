namespace MyFramework
{
    public interface IPair<TFirst, TSecond>
    {
        TFirst First { get; set; }
        TSecond Second { get; set; }
    }
}