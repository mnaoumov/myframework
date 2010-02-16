namespace MyFramework.Delegates
{
    public class Delegate<TArgument, TReturn> : BaseDelegate
    {
        public delegate TReturn InnerDelegate(TArgument argument);

        private readonly InnerDelegate innerDelegate;

        public Delegate(InnerDelegate innerDelegate)
        {
            this.innerDelegate = innerDelegate;
        }

        public TReturn Invoke(TArgument argument)
        {
           return innerDelegate(argument);
        }

        public static implicit operator Delegate<TArgument, TReturn> (TReturn constantValue)
        {
            return GetConstantDelegate(constantValue);
        }

        public static Delegate<TArgument, TReturn> GetConstantDelegate(TReturn constantValue)
        {
            return new Delegate<TArgument, TReturn>(delegate { return constantValue;});
        }
    }
}