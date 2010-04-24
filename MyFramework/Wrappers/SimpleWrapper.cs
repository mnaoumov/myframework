using MyFramework.Helpers;

namespace MyFramework.Wrappers
{
    public class SimpleWrapper<T> : Object, IWrapper<T>
    {
        private T value;

        public SimpleWrapper()
            : this(default(T))
        {
        }

        public SimpleWrapper(T value)
        {
            this.value = value;
        }

        public virtual T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public override Bool MyEquals(object obj)
        {
            return MyEquals(obj as SimpleWrapper<T>);
        }

        public Bool MyEquals(SimpleWrapper<T> obj)
        {
            return ObjectsHelper.AreEqual(value, obj.value);
        }

        public override String MyToString()
        {
            return String.MyToString(value);
        }

        public static implicit operator T(SimpleWrapper<T> wrapper)
        {
            return wrapper.Value;
        }

        public static implicit operator SimpleWrapper<T>(T value)
        {
            return new SimpleWrapper<T>(value);
        }

    }
}