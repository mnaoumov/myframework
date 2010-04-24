using System;
using System.Collections.Generic;
using Core.Delegates;
using Core.Wrappers;

namespace Core.Utils
{
    public static class EnumerableUtils
    {
        public static Bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                return Bool.False;
            }

            return Bool.True;
        }

        public static IEnumerable<T> Empty<T>()
        {
            yield break;
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            foreach (var item in enumerable)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (var sourceItem in source)
            {
                yield return selector(sourceItem);
            }
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                return item;
            }

            return default(T);
        }

        internal static T[] ToDotNetArray<T>(this IEnumerable<T> list)
        {
            return new List<T>(list).ToArray();
        }

        public static Bool Contains<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Any(item1 => Equals(item1, item));
        }

        public static Bool Any<T>(this IEnumerable<T> enumerable, System.Predicate<T> predicate)
        {
            foreach (var item in enumerable)
            {
                if (predicate(item))
                {
                    return Bool.True;
                }
            }

            return Bool.False;
        }

        public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> enumerable, System.Predicate<T> predicate)
        {
            foreach (var item in enumerable)
            {
                if (!predicate(item))
                {
                    yield break;
                }
                
                yield return item;
            }
        }

        public static IEnumerable<T> Join<T>(this IEnumerable<IEnumerable<T>> enumerables)
        {
            return from enumerable in enumerables
                   from item in enumerable
                   select item;
        }

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            foreach (var item in source)
            {
                foreach (var collection in collectionSelector(item))
                {
                    yield return resultSelector(item, collection);
                }
            }
        }

        public static T Aggregate<T>(this IEnumerable<T> enumerable,Aggregator<T> aggregator)
        {
            var resultWrapper = new Wrapper<T>();

            foreach (var item in enumerable)
            {
                resultWrapper = !resultWrapper.HasValue ? item : aggregator(resultWrapper, item);
            }

            return resultWrapper;
        }

        public static Bool SequentialEquals<T>(this IEnumerable<T> enumerable1,IEnumerable<T> enumerable2)
        {
            var enumerator2 = enumerable2.GetEnumerator();

            foreach (var item in enumerable1)
            {
                if (!enumerator2.MoveNext() || !Object.MyEquals(item, enumerator2.Current))
                {
                    return Bool.False;
                }
            }
            
            return !enumerator2.MoveNext();
        }

        public static String EnumerableToString<T>(this IEnumerable<T> enumerable)
        {
            var itemStrings = from item in enumerable select String.ToString(item);

            return String.Concat("{", itemStrings.Aggregate((string1, string2) => String.Concat(string1, ",", string2)),
                                 "}");
        }
    }
}