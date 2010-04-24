using System;

namespace MyFramework.Wrappers
{
    public abstract class EnumWrapper : Object
    {
        public override String MyToString()
        {
            return InnerEnum.ToString();
        }

        protected abstract Enum InnerEnum { get; }
    }
}