using System;
using System.Runtime.Serialization;

namespace KafkaInfrastructure.Producer.Domain
{
    [Serializable]
    public class CustomMessageProducerException : Exception
    {

        public CustomMessageProducerException()
        {
        }



        public CustomMessageProducerException(string message) : base(message)
        {
        }

        public CustomMessageProducerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomMessageProducerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
