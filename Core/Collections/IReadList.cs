using System.Collections.Generic;

namespace Core.Collections
{
    public interface IReadList<T>:IEnumerable<T>
    {
        Bool Contains(T item);
    }
}