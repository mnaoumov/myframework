namespace MyFramework.Delegates
{
    public class GeneralTransformer : Transformer<object, object>
    {
        private static readonly GeneralTransformer easyTransformer = new GeneralTransformer(EasyTransform);

        public GeneralTransformer(Delegate<object, object>.InnerDelegate innerDelegate)
            : base(innerDelegate)
        {
        }

        public static GeneralTransformer EasyTransformer
        {
            get { return easyTransformer; }
        }

        private static object EasyTransform(object argument)
        {
            return argument;
        }

        public static GeneralTransformer From<TIn, TOut>(Delegate<TIn, TOut>.InnerDelegate innerDelegate)
        {
            return From(new Transformer<TIn, TOut>(innerDelegate));
        }

        public static GeneralTransformer From<TIn, TOut>(Transformer<TIn, TOut> transformer)
        {
            return new GeneralTransformer(delegate(object argument) { return transformer.Transform((TIn) argument); });
        }

        public Transformer<TIn, TOut> To<TIn, TOut>()
        {
            return new Transformer<TIn, TOut>(delegate(TIn argument) { return (TOut) Transform(argument); });
        }
    }
}