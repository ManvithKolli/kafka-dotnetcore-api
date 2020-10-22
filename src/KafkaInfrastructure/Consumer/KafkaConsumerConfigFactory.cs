using Confluent.Kafka;
using KafkaInfrastructure.Consumer.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KafkaInfrastructure.Consumer
{
    public class KafkaConsumerConfigFactory : IKafkaConsumerConfigFactory
    {
        private readonly ILogger<KafkaConsumerConfigFactory> logger;
        private readonly KafkaServerConfiguration kafkaServerConfiguration;
        private readonly KafkaConsumerConfiguration kafkaConsumerConfiguration;

        public KafkaConsumerConfigFactory(ILogger<KafkaConsumerConfigFactory> logger, KafkaServerConfiguration kafkaServerConfiguration,
            KafkaConsumerConfiguration kafkaConsumerConfiguration)
        {
            this.logger = logger;
            this.kafkaServerConfiguration = kafkaServerConfiguration;
            this.kafkaConsumerConfiguration = kafkaConsumerConfiguration;
        }
        public ConsumerConfig CreateConsumerConfig()
        {
            AutoOffsetReset offsetReset = AutoOffsetReset.Latest;
            Enum.TryParse<AutoOffsetReset>(kafkaConsumerConfiguration.AutoOffsetReset, true, out offsetReset);

            IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted;
            Enum.TryParse<IsolationLevel>(kafkaConsumerConfiguration.IsolationLevel, true, out isolationLevel);

            return new ConsumerConfig()
            {
                BootstrapServers = kafkaServerConfiguration.BootstrapServers,
                SaslKerberosPrincipal = kafkaServerConfiguration.SaslKerberosPrincipal,
                SaslKerberosServiceName = kafkaServerConfiguration.SaslKerberosServiceName,
                SslCaLocation = kafkaServerConfiguration.SslCaLocation,
                SaslKerberosKeytab = kafkaServerConfiguration.SaslKerberosKeytab,
                SaslMechanism = kafkaServerConfiguration.SaslMechanism,
                SecurityProtocol = kafkaServerConfiguration.SecurityProtocol,
                SaslKerberosKinitCmd = kafkaServerConfiguration.SaslKerberosKinitCmd,

                GroupId = kafkaConsumerConfiguration.GroupId,
                AutoOffsetReset = offsetReset,
                IsolationLevel = isolationLevel,
                EnableAutoCommit = kafkaConsumerConfiguration.EnableAutoCommit,
                AutoCommitIntervalMs = kafkaConsumerConfiguration.AutoCommitIntervalMs,
                EnableAutoOffsetStore = kafkaConsumerConfiguration.EnableAutoOffsetStore,
                EnablePartitionEof = kafkaConsumerConfiguration.EnablePartitionEof,
                CheckCrcs = kafkaConsumerConfiguration.CheckCrcs,
                MaxPollIntervalMs = kafkaConsumerConfiguration.MaxPollIntervalMs,
                HeartbeatIntervalMs = kafkaConsumerConfiguration.HeartbeatIntervalMs,
                SessionTimeoutMs = kafkaConsumerConfiguration.SessionTimeoutMs,
                FetchMinBytes = kafkaConsumerConfiguration.FetchMinBytes,
                FetchMaxBytes = kafkaConsumerConfiguration.FetchMaxBytes,
                FetchWaitMaxMs = kafkaConsumerConfiguration.FetchWaitMaxMs,
                MaxPartitionFetchBytes = kafkaConsumerConfiguration.MaxPartitionFetchBytes,
                ReconnectBackoffMs = kafkaConsumerConfiguration.ReconnectBackoffMs,
                ReconnectBackoffMaxMs = kafkaConsumerConfiguration.ReconnectBackoffMaxMs
            };
        }

        public IEnumerable<string> GetTopics()
        {
            return kafkaConsumerConfiguration.Topics.Select(x => x.TopicName);
        }

        public string GetResilientPolicy()
        {
            return kafkaConsumerConfiguration.ConsumerResilientPolicy;
        }

        public bool IsMessageLogRequired()
        {
            return kafkaConsumerConfiguration.IsMessageLogRequired;
        }

        public double GetConsumerIntervalTime()
        {
            ValidateConsumerIntervalTime(kafkaConsumerConfiguration.ConsumerIntervalTimeMilliSeconds);

            return kafkaConsumerConfiguration.ConsumerIntervalTimeMilliSeconds;
        }

        public void LogConfiguration()
        {

            logger.LogInformation($"Kafka Server Configuration: {kafkaServerConfiguration}");

            logger.LogInformation($"Kafka Consumer Configuration :{kafkaConsumerConfiguration}");
        }

        private void ValidateConsumerIntervalTime(double consumerIntervalTimeSeconds)
        {
            if (consumerIntervalTimeSeconds > kafkaConsumerConfiguration.MaxPollIntervalMs - kafkaConsumerConfiguration.MaxPollBufferTimeMilliSeconds)
                throw new ArgumentException(nameof(consumerIntervalTimeSeconds));
        }
    }
}
