using System.Collections;
using System.Collections.Generic;
using MyFramework.Helpers;
using MyFramework.Wrappers;

namespace MyFramework.Delegates
{
    public class GeneralPredicate : BaseDelegate
    {
        private readonly Delegate<IEnumerable, Bool> @delegate;

        public GeneralPredicate(Delegate<IEnumerable, Bool> @delegate)
        {
            this.@delegate = @delegate;
        }

        public GeneralPredicate(Delegate<IEnumerable, Bool>.InnerDelegate innerDelegate)
            : this(new Delegate<IEnumerable, Bool>(innerDelegate))
        {
        }

        public Bool Check(IEnumerable objects)
        {
            return @delegate.Invoke(new UniversalWrapper(objects));
        }

        public static GeneralPredicate operator !(GeneralPredicate predicate)
        {
            return Not(predicate);
        }

        public static GeneralPredicate operator |(GeneralPredicate leftPredicate, GeneralPredicate rightPredicate)
        {
            return Or(leftPredicate, rightPredicate);
        }

        private static GeneralPredicate Or(params GeneralPredicate[] predicates)
        {
            return Or(CollectionsHelper.Iterate(predicates));
        }

        private static GeneralPredicate Or(IEnumerable<GeneralPredicate> predicates)
        {
            return
                new GeneralPredicate(
                    delegate(IEnumerable argument) { return Bool.Or(CheckAllPredicates(predicates, argument)); });
        }

        public static GeneralPredicate operator &(GeneralPredicate leftPredicate, GeneralPredicate rightPredicate)
        {
            return And(leftPredicate, rightPredicate);
        }

        private static GeneralPredicate And(params GeneralPredicate[] predicates)
        {
            return And(CollectionsHelper.Iterate(predicates));
        }

        private static GeneralPredicate And(IEnumerable<GeneralPredicate> predicates)
        {
            return
                new GeneralPredicate(
                    delegate(IEnumerable arguments) { return Bool.And(CheckAllPredicates(predicates, arguments)); });
        }


        private static IEnumerable<Bool> CheckAllPredicates(IEnumerable<GeneralPredicate> predicates,
                                                            IEnumerable arguments)
        {
            return CollectionsHelper.CheckAll(predicates, GetPredicateCheckPredicate(arguments));
        }

        private static Predicate<GeneralPredicate> GetPredicateCheckPredicate(IEnumerable arguments)
        {
            return
                new Predicate<GeneralPredicate>(
                    delegate(GeneralPredicate predicate) { return predicate.Check(arguments); });
        }

        private static GeneralPredicate Not(GeneralPredicate predicate)
        {
            return new GeneralPredicate(delegate(IEnumerable arguments) { return !predicate.Check(arguments); });
        }

        public static implicit operator GeneralPredicate(Bool @bool)
        {
            return new GeneralPredicate(Delegate<IEnumerable, Bool>.GetConstantDelegate(@bool));
        }

        public Transformer<IEnumerable, Bool> GetTransformer()
        {
            return new Transformer<IEnumerable, Bool>(Check);
        }

        public IEnumerable<Bool> CheckAll(IEnumerable<IEnumerable> enumerable)
        {
            return CollectionsHelper.CheckAll(enumerable, this);
        }
    }
}