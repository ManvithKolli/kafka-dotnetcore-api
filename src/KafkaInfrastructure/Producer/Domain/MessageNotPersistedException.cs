using System;
using System.Runtime.Serialization;

namespace KafkaInfrastructure.Producer.Domain
{
    [Serializable]
    internal class MessageNotPersistedException : Exception
    {
        public MessageNotPersistedException()
        {
        }

        public MessageNotPersistedException(string message) : base(message)
        {
        }

        public MessageNotPersistedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MessageNotPersistedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
