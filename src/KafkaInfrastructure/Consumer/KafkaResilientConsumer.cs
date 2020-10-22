using KafkaInfrastructure.Consumer.Domain;
using Polly;
using Polly.Registry;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaInfrastructure.Consumer
{
    public class KafkaResilientConsumer : IKafkaResilientConsumer
    {
        private readonly IKafkaConsumerConfigFactory kafkaConsumerConfigFactory;
        private readonly IHostedKafkaConsumerProcessor kafkaHostedProcessor;
        private readonly KafkaConsumerConfiguration kafkaConsumerConfiguration;
        private readonly IReadOnlyPolicyRegistry<string> registry;

        public KafkaResilientConsumer(IKafkaConsumerConfigFactory kafkaConsumerConfigFactory, IHostedKafkaConsumerProcessor kafkaHostedProcessor,
            KafkaConsumerConfiguration kafkaConsumerConfiguration, IReadOnlyPolicyRegistry<string> registry)
        {
            this.kafkaConsumerConfigFactory = kafkaConsumerConfigFactory;
            this.kafkaHostedProcessor = kafkaHostedProcessor;
            this.kafkaConsumerConfiguration = kafkaConsumerConfiguration;
            this.registry = registry;
        }

        public async Task Execute(CancellationToken stoppingToken)
        {
            var policy = registry.Get<AsyncPolicy>(kafkaConsumerConfigFactory.GetResilientPolicy());

            if (kafkaConsumerConfiguration.IsConsumerResilienceRequired)
                await policy.ExecuteAsync(() => kafkaHostedProcessor.BeginKafkaConsumption(stoppingToken));

            await kafkaHostedProcessor.BeginKafkaConsumption(stoppingToken);
        }
    }
}
