using DotNet = System;

namespace Core.Exceptions
{
    [DotNet.Serializable]
    public class ArgumentException : MyBaseException
    {
        public ArgumentException()
        {
        }

        public ArgumentException(String message)
            : base(message)
        {
        }

        public ArgumentException(String message, DotNet.Exception inner)
            : base(message, inner)
        {
        }

        protected ArgumentException(
            DotNet.Runtime.Serialization.SerializationInfo info,
            DotNet.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}