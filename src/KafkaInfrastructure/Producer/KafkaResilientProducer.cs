using KafkaInfrastructure.Producer.Domain;
using Polly;
using Polly.Registry;
using System.Threading.Tasks;

namespace KafkaInfrastructure.Producer
{
    public class KafkaResilientProducer : IKafkaResilientProducer
    {
        private readonly IKafkaProducer kafkaProducer;
        private readonly IKafkaProducerConfigFactory kafkaProducerConfigFactory;
        private readonly IReadOnlyPolicyRegistry<string> registry;

        public KafkaResilientProducer(IKafkaProducer kafkaProducer, IKafkaProducerConfigFactory kafkaProducerConfigFactory,
            IReadOnlyPolicyRegistry<string> registry)
        {
            this.kafkaProducer = kafkaProducer;
            this.kafkaProducerConfigFactory = kafkaProducerConfigFactory;
            this.registry = registry;
        }

        public async Task<DeliveryResult> Produce(CustomMessage message)
        {
            var policy = registry.Get<AsyncPolicy>(kafkaProducerConfigFactory.GetResilientPolicy());

            if (kafkaProducerConfigFactory.IsProducerResilienceRequired())
                return await policy.ExecuteAsync(() => kafkaProducer.Produce(message));

            return await kafkaProducer.Produce(message);
        }
    }
}
