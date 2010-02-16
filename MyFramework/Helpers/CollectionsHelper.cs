using System.Collections;
using System.Collections.Generic;
using MyFramework.Delegates;
using MyFramework.Exceptions;

namespace MyFramework.Helpers
{
    public static class CollectionsHelper
    {
        public static Bool AreEqual(IEnumerable leftCollection, IEnumerable rightCollection)
        {
            if (ReferenceEquals(leftCollection, rightCollection))
            {
                return Bool.True;
            }

            if (leftCollection == null || rightCollection == null)
            {
                return Bool.False;
            }

            IEnumerator rightEnumerator = rightCollection.GetEnumerator();

            foreach (object leftItem in leftCollection)
            {
                if (!rightEnumerator.MoveNext())
                {
                    return Bool.False;
                }

                object rightItem = rightEnumerator.Current;

                if (ObjectsHelper.AreEqual(leftItem, rightItem))
                {
                    continue;
                }

                return Bool.False;
            }

            return !rightEnumerator.MoveNext();
        }

        public static IEnumerable<T> GetEmptyEnumerable<T>()
        {
            yield break;
        }

        public static IEnumerable<T> Iterate<T>(params T[] items)
        {
            return items;
        }

        public static T[] CreateArray<T>(params T[] items)
        {
            return items;
        }

        public static IEnumerable<TOut> Transform<TIn, TOut>(IEnumerable<TIn> sources,
                                                             Transformer<TIn, TOut> transformer)
        {
            foreach (TIn source in sources)
            {
                yield return transformer.Transform(source);
            }
        }

        public static IEnumerable<TOut> Transform<TIn, TOut>(IEnumerable<TIn> sources)
        {
            return Transform(sources, Transformer<TIn, TOut>.GetDefaultTransformer());
        }

        public static Bool ForOne(IEnumerable enumerable, Delegates.Predicate<object> predicate)
        {
            return ForOne(ConvertToGenericEnumerable(enumerable), predicate);
        }

        public static IEnumerable<object> ConvertToGenericEnumerable(IEnumerable enumerable)
        {
            foreach (object item in enumerable)
            {
                yield return item;
            }
        }

        public static Bool ForOne<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate)
        {
            return Bool.Or(CheckAll(enumerable, predicate));
        }

        public static Bool ForAll(IEnumerable enumerable, Delegates.Predicate<object> predicate)
        {
            return ForAll(ConvertToGenericEnumerable(enumerable), predicate);
        }

        public static Bool ForAll<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate)
        {
            return Bool.And(CheckAll(enumerable, predicate));
        }

        public static IEnumerable<Bool> CheckAll<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate)
        {
            return Transform(enumerable, predicate.GetTransformer());
        }

        public static IEnumerable<Bool> CheckAll(IEnumerable<IEnumerable> enumerable, GeneralPredicate predicate)
        {
            return Transform(enumerable, predicate.GetTransformer());
        }

        public static Bool ContainsElement<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate, out T firstElement)
        {
            foreach (T item in enumerable)
            {
                if (predicate.Check(item))
                {
                    firstElement = item;
                    return Bool.True;
                }
            }

            firstElement = default(T);
            return Bool.False;
        }

        public static Bool ContainsElement<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate)
        {
            T firstElement;

            return ContainsElement(enumerable, predicate, out firstElement);
        }

        public static Bool TryGetFirstElement<T>(IEnumerable<T> enumerable, out T firstElement)
        {
            return ContainsElement<T>(enumerable, Bool.True, out firstElement);
        }

        public static Bool TryGetFirstElement(IEnumerable enumerable, out object firstElement)
        {
            return TryGetFirstElement(ConvertToGenericEnumerable(enumerable), out firstElement);
        }

        public static Bool IsEmpty(IEnumerable enumerable)
        {
            object firstElement;

            return !TryGetFirstElement(ConvertToGenericEnumerable(enumerable), out firstElement);
        }

        public static IEnumerable<T> FilterWhile<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate)
        {
            foreach (T item in enumerable)
            {
                if (!predicate.Check(item))
                {
                    break;
                }

                yield return item;
            }
        }

        public static IEnumerable<T> Filter<T>(IEnumerable<T> enumerable, Delegates.Predicate<T> predicate)
        {
            foreach (T item in enumerable)
            {
                if (!predicate.Check(item))
                {
                    continue;
                }

                yield return item;
            }
        }

        public static IEnumerable<T> Join<T>(params IEnumerable<T>[] enumerables)
        {
            return Join(Iterate(enumerables));
        }

        public static IEnumerable<T> Join<T>(IEnumerable<IEnumerable<T>> enumerables)
        {
            foreach (IEnumerable<T> enumerable in enumerables)
            {
                foreach (T item in enumerable)
                {
                    yield return item;
                }
            }
        }

        public static TResult ChainTransform<TSource, TResult>(IEnumerable<TSource> items, TResult initialValue, ChainTransformer<TSource, TResult> chainTransformer)
        {
            TResult result = initialValue;

            ForEach(items,
                    new Delegates.Action<TSource>(delegate(TSource item) { result = chainTransformer.ChainTransform(result, item); }));

            return result;
        }

        public static TResult ChainTransform<TResult>(IEnumerable<TResult> items, ChainTransformer<TResult> chainTransformer)
        {
            TResult firstElement;

            if (!TryGetFirstElement(items, out firstElement))
            {
                return default(TResult);
            }

            IEnumerable<TResult> itemsExceptFirst = GetItemsExceptFirst(items);

            return ChainTransform(itemsExceptFirst, firstElement, chainTransformer);
        }

        public static IEnumerable<T> GetItemsExceptFirst<T>(IEnumerable<T> items)
        {
            IEnumerator<T> enumerator = items.GetEnumerator();

            enumerator.MoveNext();

            return Iterate(enumerator);
        }

        public static IEnumerable<T> Iterate<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static void ForEach<T>(IEnumerable<T> items, Delegates.Action<T> action)
        {
            foreach (T item in items)
            {
                action.Do(item);
            }
        }

        public static String MyToString(IEnumerable enumerable)
        {
            Char openBracket = '{';
            Char closedBracket = '}';

            String chainTransform = ChainTransform(String.EnumerableToMyString(enumerable), new ChainTransformer<String>(delegate(String result, String item)
            {
                return
                    result +
                    ", " +
                    item;
            }));
            return openBracket + chainTransform + closedBracket;
        }

        public static T GetFirstElement<T>(IEnumerable<T> enumerable)
        {
            T firstElement;
            
            if (TryGetFirstElement<T>(enumerable, out firstElement))
            {
                return firstElement;
            }

            throw new CollectionIsEmptyException();
        }

        public static object GetFirstElement(IEnumerable enumerable)
        {
            return GetFirstElement(ConvertToGenericEnumerable(enumerable));
        }
    }
}