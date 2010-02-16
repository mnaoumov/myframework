using System;
using System.Collections;
using System.Collections.Generic;
using MyFramework.Delegates;
using MyFramework.Helpers;

namespace MyFramework.Math.Sets
{
    public class FiniteSet<T> : CheckedSet<T>, IEnumerable<T>
    {
        private readonly LinkedList<T> elements;

        private FiniteSet(IEnumerable<T> elements):base(CreateCondition(elements))
        {
            this.elements = new LinkedList<T>(elements);
        }

        private static CheckedCondition<T> CreateCondition(IEnumerable<T> elements)
        {
            return
                CheckedCondition<T>.Or(CollectionsHelper.Transform(elements,new Transformer<T, Condition<T>>(
                                                                                        Condition<T>.ConditionForElement)));
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static FiniteSet<T> Create(IEnumerable<T> elements)
        {
            return new FiniteSet<T>(elements);
        }

        public static FiniteSet<T> Create(params T[] elements)
        {
            return Create(CollectionsHelper.Iterate(elements));
        }
    }
}