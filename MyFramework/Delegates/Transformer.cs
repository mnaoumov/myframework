using System;
using MyFramework.Helpers;

namespace MyFramework.Delegates
{
    public class Transformer<TIn, TOut> : BaseDelegate
    {
        private readonly Delegate<TIn, TOut> innerDelegate;

        public Transformer(Delegate<TIn, TOut>.InnerDelegate innerDelegate)
        {
            this.innerDelegate = new Delegate<TIn, TOut>(innerDelegate);
        }

        public TOut Transform(TIn source)
        {
            return innerDelegate.Invoke(source);
        }

        public static Transformer<TIn, TOut> GetDefaultTransformer()
        {
            Transformer<TIn, TOut> transformer;

            if (ConvertHelper.TryGetTransformer(out transformer))
            {
                return transformer;
            }

            throw new InvalidCastException(String.Format("Cannot transform {A} into {B}.",
                                                              new String.ParametersPair("A", Type.GetTypeName<TIn>()),
                                                              new String.ParametersPair("B", Type.GetTypeName<TOut>())));
        }
    }
}