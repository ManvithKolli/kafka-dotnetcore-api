using Confluent.Kafka;
using KafkaInfrastructure.Producer.Domain;
using System;

namespace KafkaInfrastructure.Producer
{
    public class KafkaProducerConfigFactory : IKafkaProducerConfigFactory
    {
        private readonly KafkaServerConfiguration kafkaServerConfiguration;
        private readonly KafkaProducerConfiguration kafkaProducerConfiguration;

        public KafkaProducerConfigFactory(KafkaServerConfiguration kafkaServerConfiguration,
            KafkaProducerConfiguration kafkaProducerConfiguration)
        {
            this.kafkaServerConfiguration = kafkaServerConfiguration;
            this.kafkaProducerConfiguration = kafkaProducerConfiguration;
        }

        public ProducerConfig CreateProducerConfig()
        {
            Acks acks = Acks.Leader;

            Enum.TryParse<Acks>(kafkaProducerConfiguration.Acks, true, out acks);

            return new ProducerConfig()
            {
                BootstrapServers = kafkaServerConfiguration.BootstrapServers,
                SaslKerberosPrincipal = kafkaServerConfiguration.SaslKerberosPrincipal,
                SaslKerberosServiceName = kafkaServerConfiguration.SaslKerberosServiceName,
                SslCaLocation = kafkaServerConfiguration.SslCaLocation,
                SaslKerberosKeytab = kafkaServerConfiguration.SaslKerberosKeytab,
                SaslMechanism = kafkaServerConfiguration.SaslMechanism,
                SecurityProtocol = kafkaServerConfiguration.SecurityProtocol,
                SaslKerberosKinitCmd = kafkaServerConfiguration.SaslKerberosKinitCmd,

                Acks = acks,
                QueueBufferingMaxKbytes = kafkaProducerConfiguration.QueueBufferingMaxKbytes,
                QueueBufferingMaxMessages = kafkaProducerConfiguration.QueueBufferingMaxMessages,
                MessageTimeoutMs = kafkaProducerConfiguration.MessageTimeoutMs,
                RequestTimeoutMs = kafkaProducerConfiguration.RequestTimeoutMs,
                LingerMs = kafkaProducerConfiguration.LingerMs,
                EnableIdempotence = kafkaProducerConfiguration.EnableIdempotence,
                MessageSendMaxRetries = kafkaProducerConfiguration.MessageSendMaxRetries,
                RetryBackoffMs = kafkaProducerConfiguration.RetryBackoffMs,
                BatchNumMessages = kafkaProducerConfiguration.BatchNumMessages
            };
        }

        public string GetResilientPolicy()
        {
            return kafkaProducerConfiguration.ProducerResilientPolicy;
        }

        public bool IsProducerResilienceRequired()
        {
            return kafkaProducerConfiguration.IsProducerResilienceRequired;
        }
    }
}
