using Confluent.Kafka;
using KafkaInfrastructure.Resiliency.Domain;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace KafkaInfrastructure.Resiliency.KafkaResiliencyPolicies
{
    public class KafkaConsumeRetryPolicy : IAsyncResiliencyPolicyFactory
    {
        private readonly ILogger<KafkaConsumeRetryPolicy> logger;
        private readonly ResiliencyConfiguration kafkaResiliencyConfiguration;

        public KafkaConsumeRetryPolicy(ResiliencyConfiguration kafkaResiliencyConfiguration)
        {
            this.logger = this.logger = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Default", LogLevel.Information);

            }).CreateLogger<KafkaConsumeRetryPolicy>();

            this.kafkaResiliencyConfiguration = kafkaResiliencyConfiguration;

        }

        public AsyncPolicy CreateResilientPolicy()
        {
            return Policy.Handle<ConsumeException>()
                .Or<KafkaException>()
                .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(kafkaResiliencyConfiguration.messagingKafkaConsumeRetryTimeSpan),
                (exception, timeSpan) =>
                {
                    logger.LogInformation($"Executing KafkaConsumeRetryPolicy, Retrying after {timeSpan.TotalSeconds} seconds time due to exception {exception.ToString()} ");
                }
                );
        }
    }
}
