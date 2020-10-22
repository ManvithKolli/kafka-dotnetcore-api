using Confluent.Kafka;

namespace KafkaInfrastructure.Producer.Domain
{
    public class DeliveryResultAssembler : IDeliveryResultAssembler
    {
        public DeliveryResult Assemble(DeliveryResult<string, string> deliveryResult)
        {
            return new DeliveryResult()
            {
                topic = deliveryResult.Topic,
                partition = deliveryResult.Partition.Value,
                offset = deliveryResult.Offset.Value,
                persistenceStatus = deliveryResult.Status.ToString()
            };
        }
    }
}
