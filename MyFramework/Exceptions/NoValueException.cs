using System;
using System.Runtime.Serialization;

namespace MyFramework.Exceptions
{
    [Serializable]
    public class NoValueException : Exception
    {
        public NoValueException()
        {
        }

        public NoValueException(String message) : base(message)
        {
        }

        public NoValueException(String message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}