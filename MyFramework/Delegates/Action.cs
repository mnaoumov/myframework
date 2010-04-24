namespace MyFramework.Delegates
{
    public class Action<TArgument> : BaseDelegate
    {
        public delegate void InnerDelegate(TArgument argument);

        private readonly InnerDelegate innerDelegate;

        public Action(InnerDelegate innerDelegate)
        {
            this.innerDelegate = innerDelegate;
        }

        public void Do(TArgument argument)
        {
            innerDelegate(argument);
        }
    }
}