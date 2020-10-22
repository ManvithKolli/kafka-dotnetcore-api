using Confluent.Kafka;

namespace KafkaInfrastructure.Producer.Domain
{
    public interface IDeliveryResultAssembler
    {
        DeliveryResult Assemble(DeliveryResult<string, string> deliveryResult);
    }
}
