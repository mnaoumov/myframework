using System.Collections;
using MyFramework.Helpers;

namespace MyFramework.Wrappers
{
    public class UniversalWrapper:EnumerableWrapper<object>
    {
        public UniversalWrapper(IEnumerable enumerable) : base(CollectionsHelper.ConvertToGenericEnumerable(enumerable))
        {
        }

        public UniversalWrapper(params object[] objects) : this(CollectionsHelper.Iterate(objects))
        {}

        public static implicit operator UniversalWrapper(object[] objects)
        {
            return new UniversalWrapper(objects);
        }
    }
}