using System;
using MyFramework.Delegates;

namespace MyFramework.Wrappers
{
    public class LazyWrapper<T> : HasValueWrapper<T>
    {
        public LazyWrapper()
        {
        }

        public LazyWrapper(T value) : base(value)
        {
        }

        public override T Value
        {
            get
            {
                if (!HasValue)
                {
                    Value = GetInitialValue();
                }

                return base.Value;
            }
            set { base.Value = value; }
        }

        protected virtual T GetInitialValue()
        {
            return default(T);  
        }

        public static LazyWrapper<T> Create(Initializer<T> initializer)
        {
            return new LazyInitializer(initializer);
        }

        public static implicit operator LazyWrapper<T>(T value)
        {
            return new LazyWrapper<T>(value);
        }

        #region Nested type: LazyInitializer

        private class LazyInitializer : LazyWrapper<T>
        {
            private readonly Initializer<T> initializer;

            public LazyInitializer(Initializer<T> initializer)
            {
                this.initializer = initializer;
            }

            protected override T GetInitialValue()
            {
                return initializer.Initialize();
            }
        }

        #endregion
    }
}