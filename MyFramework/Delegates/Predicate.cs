using System;
using System.Collections;
using System.Collections.Generic;
using MyFramework.Helpers;

namespace MyFramework.Delegates
{
    public class Predicate<T> : GeneralPredicate
    {
        public Predicate(Delegate<T, Bool> @delegate):base(PrepareGeneralPredicateDelegate(@delegate))
        {
        }

        private static Delegate<IEnumerable, Bool>.InnerDelegate PrepareGeneralPredicateDelegate(Delegate<T, Bool> @delegate)
        {
            return delegate(IEnumerable arguments)
                       {
                           return @delegate.Invoke((T) CollectionsHelper.GetFirstElement(arguments));
                       };
        }

        public Predicate(Delegate<T, Bool>.InnerDelegate innerDelegate)
            : this(new Delegate<T, Bool>(innerDelegate))
        {
        }

        public Bool Check(T obj)
        {
            return Check(CollectionsHelper.Iterate(obj));
        }

        public static Predicate<T> operator !(Predicate<T> predicate)
        {
            return Not(predicate);
        }

        public static Predicate<T> operator |(Predicate<T> leftPredicate, Predicate<T> rightPredicate)
        {
            return Or(leftPredicate, rightPredicate);
        }

        private static Predicate<T> Or(params Predicate<T>[] predicates)
        {
            return Or(CollectionsHelper.Iterate(predicates));
        }

        private static Predicate<T> Or(IEnumerable<Predicate<T>> predicates)
        {
            return new Predicate<T>(delegate(T argument) { return Bool.Or(CheckAllPredicates(predicates, argument)); });
        }

        public static Predicate<T> operator &(Predicate<T> leftPredicate, Predicate<T> rightPredicate)
        {
            return And(leftPredicate, rightPredicate);
        }

        private static Predicate<T> And(params Predicate<T>[] predicates)
        {
            return And(CollectionsHelper.Iterate(predicates));
        }

        private static Predicate<T> And(IEnumerable<Predicate<T>> predicates)
        {
            return new Predicate<T>(delegate(T argument) { return Bool.And(CheckAllPredicates(predicates, argument)); });
        }


        private static IEnumerable<Bool> CheckAllPredicates(IEnumerable<Predicate<T>> predicates, T argument)
        {
            return CollectionsHelper.CheckAll(predicates, GetPredicateCheckPredicate(argument));
        }

        private static Predicate<Predicate<T>> GetPredicateCheckPredicate(T argument)
        {
            return new Predicate<Predicate<T>>(delegate(Predicate<T> predicate) { return predicate.Check(argument); });
        }

        private static Predicate<T> Not(Predicate<T> predicate)
        {
            return new Predicate<T>(delegate(T argument) { return !predicate.Check(argument); });
        }

        public static implicit operator Predicate<T>(Bool @bool)
        {
            return new Predicate<T>(Delegate<T, Bool>.GetConstantDelegate(@bool));
        }

        public Transformer<T, Bool> GetTransformer()
        {
            return new Transformer<T, Bool>(Check);
        }

        public IEnumerable<Bool> CheckAll(IEnumerable<T> enumerable)
        {
            return CollectionsHelper.CheckAll(enumerable, this);
        }
    }
}