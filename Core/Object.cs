using System;
using DotNet = System;

namespace Core
{
    public class Object : System.Object
    {
        public override sealed bool Equals(System.Object obj)
        {
            return MyEquals(this, obj);
        }

        public static Bool MyEquals(System.Object obj1, System.Object obj2)
        {
            if (EqualsByReference(obj1, obj2))
            {
                return Bool.True;
            }

            if (IsNull(obj1) || IsNull(obj2))
            {
                return Bool.False;
            }

            var obj1AsObject = obj1 as Object;

            if (obj1AsObject == null)
            {
                return Equals(obj1, obj2);
            }

            return obj1AsObject.MyEquals(obj2);
        }

        public static Bool IsNull(System.Object obj)
        {
            return EqualsByReference(obj, null);
        }

        public override int GetHashCode()
        {
            const int defaultHashCode = 0;

            return defaultHashCode;
        }

        public virtual Bool MyEquals(System.Object obj)
        {
            return EqualsByReference(this, obj);
        }

        public static Bool EqualsByReference(System.Object obj1, System.Object obj2)
        {
            return ReferenceEquals(obj1, obj2);
        }

        public Type MyGetType()
        {
            return Type.GetType(this);
        }

        public override sealed string ToString()
        {
            return String.ToDotNetString(this);
        }

        public virtual String MyToString()
        {
            return MyGetType().MyToString();
        }
    }
}