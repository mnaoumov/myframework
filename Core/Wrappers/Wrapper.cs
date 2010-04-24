using Core.Exceptions;

namespace Core.Wrappers
{
    public class Wrapper<T> : Object
    {
        private T _value;

        public Wrapper()
        {
            HasValue = Bool.False;
        }

        public Wrapper(T value)
        {
            Value = value;
        }

        public virtual T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new ValueIsNotSetException();
                }

                return _value;
            }
            set
            {
                _value = value;
                HasValue = Bool.True;
            }
        }

        public Bool HasValue { get; private set; }

        public static implicit operator T(Wrapper<T> wrapper)
        {
            return wrapper.Value;
        }

        public static implicit operator Wrapper<T>(T value)
        {
            return new Wrapper<T>(value);
        }

    }
}