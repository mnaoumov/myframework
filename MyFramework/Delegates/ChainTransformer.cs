namespace MyFramework.Delegates
{
    public class ChainTransformer<TSource, TResult> : BaseDelegate
    {
        #region Delegates

        public delegate TResult InnerDelegate(TResult result, TSource item);

        #endregion

        private readonly InnerDelegate @delegate;

        public ChainTransformer(InnerDelegate @delegate)
        {
            this.@delegate = @delegate;
        }

        public TResult ChainTransform(TResult result, TSource item)
        {
            return @delegate(result, item);
        }
    }

        public class ChainTransformer<TResult> : ChainTransformer<TResult,TResult>
        {
            public ChainTransformer(InnerDelegate @delegate) : base(@delegate)
            {
            }
        }
}