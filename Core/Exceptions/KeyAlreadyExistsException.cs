using DotNet = System;

namespace Core.Exceptions
{
    [DotNet.Serializable]
    public class KeyAlreadyExistsException : MyBaseException
    {
        public KeyAlreadyExistsException()
        {
        }

        public KeyAlreadyExistsException(DotNet.Object key)
        {
            Data.Add("key", key);
        }

        public KeyAlreadyExistsException(String message)
            : base(message.ToDotNetString())
        {
        }

        public KeyAlreadyExistsException(String message, DotNet.Exception inner)
            : base(message.ToDotNetString(), inner)
        {
        }

        protected KeyAlreadyExistsException(
            DotNet.Runtime.Serialization.SerializationInfo info,
            DotNet.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}