using System;
using System.Runtime.Serialization;

namespace WotDossier.Common.Python
{
    /// <summary>
    /// Exception thrown when something went wrong with pickling or unpickling.
    /// </summary>
    [Serializable]
    public class PickleException : Exception, ISerializable
    {
        public PickleException()
        {
        }
        public PickleException(string message)
            : base(message)
        {
        }
        public PickleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        // This constructor is needed for serialization.
        protected PickleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}