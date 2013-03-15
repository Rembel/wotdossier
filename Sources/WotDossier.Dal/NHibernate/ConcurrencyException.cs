using System;
using System.Runtime.Serialization;

namespace Croc.Aws.DataAccess.NHibernate
{
    /// <summary>
    /// The class has been added to throw exception from FlushEntityEventListener.
    /// Native NH and Spring related exceptions are forbidden here because of the class is used in Eproc servicve and as a result all appropriate related assemblies should be added to proxy usage
    /// </summary>
    [Serializable]
    public class ConcurrencyException : Exception
    {
        private readonly bool _entityDeleted;

        private const string ConcurrencyAccessErrorMessage =
            "The object '{0}' you have requested '{1}' has been modified or deleted by another transaction";

        /// <summary>
        /// Gets a value indicating whether [entity deleted].
        /// </summary>
        /// <value><c>true</c> if [entity deleted]; otherwise, <c>false</c>.</value>
        public bool EntityDeleted
        {
            get { return _entityDeleted; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// Flag EntityDeleted is set to false by default
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity id.</param>
        public ConcurrencyException(string entityName, object entityId)
            : this(entityName, entityId, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="entityDeleted">if set to <c>true</c> [entity deleted].</param>
        public ConcurrencyException(string entityName, object entityId, bool entityDeleted)
            : this(String.Format(ConcurrencyAccessErrorMessage, entityName, entityId))
        {
            _entityDeleted = entityDeleted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConcurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public ConcurrencyException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected ConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}