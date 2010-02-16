using System.Collections;
using MyFramework.Delegates;

namespace MyFramework.Helpers
{
    public static class ObjectsHelper
    {
        public static Bool AreEqual(object left, object right)
        {
            if (ReferenceEquals(left, right))
            {
                return Bool.True;
            }

            if (ContainsNull(left, right))
            {
                return Bool.False;
            }

            IEnumerable leftEnumerable = left as IEnumerable;
            IEnumerable rightEnumerable = right as IEnumerable;

            if (ContainsNull(leftEnumerable, rightEnumerable))
            {
                return Equals(left, right);
            }

            return CollectionsHelper.AreEqual(leftEnumerable, rightEnumerable);
        }

        public static Bool ContainsNull(params object[] objects)
        {
            return ContainsNull((IEnumerable) objects);
        }

        public static Bool ContainsNull(IEnumerable objects)
        {
            return CollectionsHelper.ForOne(objects, new Predicate<object>(delegate(object obj) { return IsNull(obj); }));
        }

        public static Bool IsNull(object obj)
        {
            return ReferenceEquals(obj, null);
        }
    }
}