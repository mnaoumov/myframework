using System.Collections;
using System.Collections.Generic;
using MyFramework.Delegates;
using MyFramework.Helpers;

namespace MyFramework.Wrappers
{
    public class EnumerableWrapper<T> : Object, IEnumerable<T>
    {
        private readonly IEnumerable<T> enumerable;

        public EnumerableWrapper(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public EnumerableWrapper<TOut> Transform<TOut>(Transformer<T, TOut> transformer)
        {
            return new EnumerableWrapper<TOut>(CollectionsHelper.Transform(this, transformer));

        }

        public EnumerableWrapper<TOut> Transform<TOut>()
        {
            return new EnumerableWrapper<TOut>(CollectionsHelper.Transform<T, TOut>(this));
        }

        public EnumerableWrapper<Bool> CheckAll(Predicate<T> predicate)
        {
            return new EnumerableWrapper<Bool>(CollectionsHelper.CheckAll(this, predicate));
        }

        public EnumerableWrapper<T> FilterWhile(Predicate<T> predicate)
        {
            return new EnumerableWrapper<T>(CollectionsHelper.FilterWhile(this, predicate));
        }

        public EnumerableWrapper<T> Filter(Predicate<T> predicate)
        {
            return new EnumerableWrapper<T>(CollectionsHelper.Filter(this, predicate));
        }

        public EnumerableWrapper<T> Join(IEnumerable<T> otherEnumerable)
        {
            return new EnumerableWrapper<T>(CollectionsHelper.Join(this, otherEnumerable));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

        public static class EnumerableWrapper
        {
            public static EnumerableWrapper<T> Join<T>(params IEnumerable<T>[] enumerables)
            {
                return new EnumerableWrapper<T>(CollectionsHelper.Join(enumerables));
            }

            public static EnumerableWrapper<T> Join<T>(IEnumerable<IEnumerable<T>> enumerables)
            {
                return new EnumerableWrapper<T>(CollectionsHelper.Join(enumerables));
            }
        }
}