using System;
using System.Runtime.Serialization;

namespace WotDossier.Dal
{
    public class PlayerInfoLoadException : Exception
    {
        public PlayerInfoLoadException()
        {
        }

        public PlayerInfoLoadException(string message) : base(message)
        {
        }

        public PlayerInfoLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlayerInfoLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
