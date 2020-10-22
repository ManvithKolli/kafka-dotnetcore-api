using Confluent.Kafka;

namespace KafkaInfrastructure.Producer.Domain
{
    public interface IKafkaProducerConfigFactory
    {
        ProducerConfig CreateProducerConfig();

        string GetResilientPolicy();

        bool IsProducerResilienceRequired();
    }
}
