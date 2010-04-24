using DotNet = System;

namespace Core.Exceptions
{
    public abstract class MyBaseException : DotNet.Exception
    {
        protected MyBaseException()
        {
        }

        protected MyBaseException(String message)
            : base(message.ToDotNetString())
        {
        }

        protected MyBaseException(String message, DotNet.Exception inner)
            : base(message.ToDotNetString(), inner)
        {
        }

        protected MyBaseException(
            DotNet.Runtime.Serialization.SerializationInfo info,
            DotNet.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public override sealed DotNet.Collections.IDictionary Data
        {
            get { return base.Data; }
        }
    }
}