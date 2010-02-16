using MyFramework.Delegates;
using MyFramework.Helpers;

namespace MyFramework
{
    public class Object
    {
        public override sealed string ToString()
        {
            return String.ToDotNetString(MyToString());
        }

        public virtual String MyToString()
        {
            return MyGetType().MyToString();
        }

        public override sealed bool Equals(object obj)
        {
            return (bool) MyEquals(obj);
        }

        public virtual Bool MyEquals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override sealed int GetHashCode()
        {
            return 0;
        }

        protected Bool MyEquals<T>(object obj, Predicate<T> predicate)
        {
            if (!(this is T) || !(obj is T))
            {
                return Bool.False;
            }

            T objAsT = (T) obj;

            if (ObjectsHelper.IsNull(objAsT))
            {
                return Bool.False;
            }

            return predicate.Check(objAsT);
        }

        public Type MyGetType()
        {
            return Type.MyGetType(this);
        }
    }
}