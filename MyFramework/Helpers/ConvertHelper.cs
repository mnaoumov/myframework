using MyFramework.Collections;
using MyFramework.Delegates;

namespace MyFramework.Helpers
{
    public static class ConvertHelper
    {
        private static readonly Pool<InOutTypePair, GeneralTransformer> pool = new Pool<InOutTypePair, GeneralTransformer>();

        public static Bool TryGetTransformer<TIn, TOut>(out Transformer<TIn, TOut> transformer)
        {
            InOutTypePair inOutTypePair = InOutTypePair.Create<TIn, TOut>();

            GeneralTransformer result = pool.GetOrAdd(inOutTypePair, PrepareTransformer);

            transformer = result.To<TIn, TOut>();

            return transformer != null;
        }

        private static GeneralTransformer PrepareTransformer(InOutTypePair inOutTypePair)
        {
            Type typeIn = inOutTypePair.TypeIn;
            Type typeOut = inOutTypePair.TypeOut;

            if (typeIn.IsChildOf(typeOut))
            {
                return GeneralTransformer.EasyTransformer;
            }

            GeneralTransformer transformer;

            if (CollectionsHelper.TryGetFirstElement(Type.GetTransformers(typeIn,typeOut), out transformer))
            {
                return transformer;
            }

            return null;
        }

        private static TOut EasyConvert<TIn, TOut>(TIn source)
        {
            return EasyConvert<TOut>(source);
        }

        private static TOut EasyConvert<TOut>(object source)
        {
            return (TOut)source;
        }

        #region Nested type: InOutTypePair

        private class InOutTypePair : Object, IPair<Type, Type>
        {
            private readonly IPair<Type, Type> innerPair;

            private InOutTypePair(IPair<Type, Type> innerPair)
            {
                this.innerPair = innerPair;
            }

            Type IPair<Type, Type>.First
            {
                get { return innerPair.First; }
                set { innerPair.First = value; }
            }

            Type IPair<Type, Type>.Second
            {
                get { return innerPair.Second; }
                set { innerPair.Second = value; }
            }

            public Type TypeIn
            {
                get { return ((IPair<Type, Type>)this).First; }
            }

            public Type TypeOut
            {
                get { return ((IPair<Type, Type>)this).Second; }
            }


            //private readonly Pair<System.Type, System.Type> innerPair;

            //private InOutTypePair(System.Type typeIn, System.Type typeOut)
            //{
            //    innerPair = new Pair<System.Type, System.Type>(typeIn, typeOut);
            //}

            //public static InOutTypePair Create<TFirst, TSecond>()
            //{
            //    return new InOutTypePair(typeof(TFirst), typeof(TSecond));
            //}

            //public override Bool MyEquals(object obj)
            //{
            //    return MyEquals(obj,
            //                    new Predicate<InOutTypePair>(
            //                        delegate(InOutTypePair argument) { return (innerPair == argument.innerPair); }));
            //}

            //private IPair<Type, Type> innerPair;


            public static InOutTypePair Create<TIn, TOut>()
            {
                return new InOutTypePair(new Pair<Type, Type>(Type.Create<TIn>(), Type.Create<TOut>()));
            }
        }

        #endregion

        public static T Convert<T>(object source)
        {
            if (source == null)
            {
                return default(T);
            }

            T converted = default(T);

            if (CollectionsHelper.ContainsElement(Type.GetTypesTree(source), new Predicate<Type>(delegate(Type sourceType) { return TryConvert(sourceType, source, out converted); })))
            {
                return converted;
            }

            throw new System.InvalidCastException();
        }

        private static Bool TryConvert<T>(Type sourceType, object source, out T converted)
        {
            Type typeOut = Type.Create<T>();

            GeneralTransformer transformer;
            
            if (CollectionsHelper.TryGetFirstElement(Type.GetTransformers(sourceType, typeOut), out transformer))
            {
                converted = (T) transformer.Transform(source);

                return Bool.True;
            }

            converted = default(T);

            return Bool.False;
        }
    }
}