using System;
using System.Runtime.Serialization;

namespace MyFramework.Exceptions
{
    [Serializable]
    public class CollectionIsEmptyException : Exception
    {
        public CollectionIsEmptyException()
        {
        }

        public CollectionIsEmptyException(String message)
            : base(message)
        {
        }

        public CollectionIsEmptyException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CollectionIsEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}