using System;
using System.Collections.Generic;
using MyFramework.Helpers;

namespace MyFramework.Math.Sets
{
    public class CheckedCondition<T>:Condition<T>
    {
        protected CheckedCondition(SetDescription description, Delegates.Predicate<T> predicate) : base(description, predicate)
        {
        }

        public static CheckedCondition<T> Or(IEnumerable<Condition<T>> conditions)
        {
            throw new NotImplementedException();
        }

        public static CheckedCondition<T> Or(params Condition<T>[] conditions)
        {
            return Or(CollectionsHelper.Iterate(conditions));
        }
    }
}