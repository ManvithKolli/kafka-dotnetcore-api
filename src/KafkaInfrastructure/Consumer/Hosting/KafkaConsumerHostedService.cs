using System;
using System.Threading;
using System.Threading.Tasks;
using KafkaInfrastructure.Consumer.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaInfrastructure.Consumer.Hosting
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly ILogger<KafkaConsumerHostedService> logger;
        private readonly IHostApplicationLifetime appLifetime;
        private readonly IKafkaResilientConsumer kafkaResilientConsumer;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger, IHostApplicationLifetime appLifetime,
            IKafkaResilientConsumer kafkaResilientConsumer)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.kafkaResilientConsumer = kafkaResilientConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Delay(5000);

                logger.LogInformation("Starting Kafka Consumer Hosted Service");

                await kafkaResilientConsumer.Execute(stoppingToken);

            }
            catch (OperationCanceledException ex)
            {
                logger.LogError($"The Kafka Consumer Hosted Service Operation has been canceled");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Exception occurred in KafkaConsumer Service: {ex.ToString()}");
            }
            finally
            {
                appLifetime.StopApplication();

                logger.LogInformation($"Stopping KafkaHostedService and Exiting");
            }
        }
    }
}
