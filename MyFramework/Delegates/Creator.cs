using MyFramework.Helpers;

namespace MyFramework.Delegates
{
    public class Creator<TKey, TProduct> : BaseDelegate
    {
        private readonly Delegate<TKey, TProduct> @delegate;

        private Creator(Delegate<TKey, TProduct> @delegate)
        {
            this.@delegate = @delegate;
        }

        public Creator(Delegate<TKey, TProduct>.InnerDelegate innerDelegate)
            : this(new Delegate<TKey, TProduct>(innerDelegate))
        {
        }

        public TProduct Create(TKey key)
        {
            return @delegate.Invoke(key);
        }
    }

    public class Creator<TProduct> : BaseDelegate
    {
        public delegate TProduct InnerCreator();

        private readonly InnerCreator creator;

        public Creator(InnerCreator creator)
        {
            this.creator = creator;
        }

        public TProduct Create()
        {
            return creator();
        }

        public Delegate<TArgument, TResult> ToDelegate<TArgument, TResult>()
        {
            return new Delegate<TArgument, TResult>(delegate { return ConvertHelper.Convert<TResult>(Create()); });
        }
    }
}