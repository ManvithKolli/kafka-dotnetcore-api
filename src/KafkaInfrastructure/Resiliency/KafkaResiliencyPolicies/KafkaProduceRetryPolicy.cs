using KafkaInfrastructure.Producer.Domain;
using KafkaInfrastructure.Resiliency.Domain;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace KafkaInfrastructure.Resiliency.KafkaResiliencyPolicies
{
    public class KafkaProduceRetryPolicy : IAsyncResiliencyPolicyFactory
    {
        private readonly ILogger<KafkaProduceRetryPolicy> logger;
        private readonly ResiliencyConfiguration kafkaResiliencyConfiguration;

        public KafkaProduceRetryPolicy(ResiliencyConfiguration kafkaResiliencyConfiguration)
        {
            this.logger = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Default", LogLevel.Information);

            }).CreateLogger<KafkaProduceRetryPolicy>();

            this.kafkaResiliencyConfiguration = kafkaResiliencyConfiguration;

        }

        public AsyncPolicy CreateResilientPolicy()
        {
            return Policy.Handle<CustomMessageProducerException>()
                .WaitAndRetryAsync(kafkaResiliencyConfiguration.messagingKafkaRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogInformation($"Executing KafkaProduceRetryPolicy, Retrying for {retryCount} time due to exception {exception.ToString()} after {timeSpan.TotalSeconds} seconds");
                }
                );
        }
    }
}
