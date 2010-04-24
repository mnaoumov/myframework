using MyFramework.Exceptions;

namespace MyFramework.Wrappers
{
    public class HasValueWrapper<T> : SimpleWrapper<T>
    {
        private Bool hasValue;

        public HasValueWrapper()
        {
            hasValue = Bool.False;
        }

        public HasValueWrapper(T value)
            : base(value)
        {
            hasValue = Bool.True;
        }

        public Bool HasValue
        {
            get { return hasValue; }
        }

        public override T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new NoValueException();
                }

                return base.Value;
            }
            set
            {
                base.Value = value;
                hasValue = Bool.True;
            }
        }
    }
}