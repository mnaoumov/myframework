using System;
using System.Runtime.Serialization;

namespace Core.Exceptions
{
    [Serializable]
    public class ValueIsNotSetException : MyBaseException
    {
        public ValueIsNotSetException()
        {
        }

        public ValueIsNotSetException(String message)
            : base(message)
        {
        }

        public ValueIsNotSetException(String message, Exception inner)
            : base(message, inner)
        {
        }

        protected ValueIsNotSetException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}