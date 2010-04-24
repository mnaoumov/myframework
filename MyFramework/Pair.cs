namespace MyFramework
{
    public class Pair<TFirst, TSecond> : Object, IPair<TFirst, TSecond>
    {
        private TFirst first;
        private TSecond second;

        public Pair(TFirst first, TSecond second)
        {
            this.first = first;
            this.second = second;
        }

        public TFirst First
        {
            get { return first; }
            set { first = value; }
        }

        public TSecond Second
        {
            get { return second; }
            set { second = value; }
        }
    }
}