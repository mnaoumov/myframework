using System;

namespace MyFramework.Math.Sets
{
    public class CheckedSet<T>:GeneralSet<T>
    {
        protected CheckedSet(CheckedCondition<T> condition)
        {
            throw new NotImplementedException();
        }
    }
}