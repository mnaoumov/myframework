namespace Core.Wrappers
{
    public abstract class LazyWrapper<T> : Wrapper<T>
    {
        public override T Value
        {
            get
            {
                if (HasValue)
                {
                    return base.Value;
                }

                Value = PrepareValue();

                return base.Value;
            }
            set { base.Value = value; }
        }

        protected abstract T PrepareValue();
    }
}