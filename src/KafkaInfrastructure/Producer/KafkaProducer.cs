using Confluent.Kafka;
using KafkaInfrastructure.Producer.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KafkaInfrastructure.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> logger;
        private readonly IKafkaProducerConfigFactory kafkaProducerConfigFactory;
        private readonly IDeliveryResultAssembler deliveryResultAssembler;

        public KafkaProducer(ILogger<KafkaProducer> logger, IKafkaProducerConfigFactory kafkaProducerConfigFactory,
            IDeliveryResultAssembler deliveryResultAssembler)
        {
            this.logger = logger;
            this.kafkaProducerConfigFactory = kafkaProducerConfigFactory;
            this.deliveryResultAssembler = deliveryResultAssembler;
        }

        public async Task<DeliveryResult> Produce(CustomMessage message)
        {
            logger.LogInformation($"Beginning Producing to Kafka Topic {message.source}");

            try
            {
                ProducerConfig producerConfiguration = kafkaProducerConfigFactory.CreateProducerConfig();

                logger.LogInformation($"Started Producing to Kakfa Topic {message.source} with message of length {message.message.Length}");

                DeliveryResult<string, string> deliveryResult = await Publish(producerConfiguration, message);

                logger.LogInformation($"Ended Producing to Kakfa Topic {message.source}");

                HandleDeliveryResult(deliveryResult);

                return deliveryResultAssembler.Assemble(deliveryResult);
            }
            catch (ProduceException<string, string> e)
            {
                string message1 = $"Producing failed with message {e.Message} || Is BrokerError:{e.Error.IsBrokerError} || IsFatal :{e.Error.IsFatal} and error code: {e.Error.Code}";
                logger.LogError(message1);
                throw new CustomMessageProducerException(message1, e);
            }
            catch (MessageNotPersistedException me)
            {
                string message1 = $"Message Not Persisted exception Occurred in kafkaProducer: {me}";
                logger.LogError(message1);
                throw new CustomMessageProducerException(message1, me);
            }
            catch (Exception ex)
            {
                string message1 = $"Exception Occurred in kafkaProcuder: {ex.ToString()}";
                logger.LogError(message1);
                throw;
            }
        }

        private async Task<DeliveryResult<string, string>> Publish(ProducerConfig producerConfiguration, CustomMessage message)
        {
            using (var producer = new ProducerBuilder<string, string>(producerConfiguration)
                    .SetLogHandler(logHandler)
                    .SetErrorHandler(errorHandler)
                    .Build())
            {
                return await producer.ProduceAsync(message.source, new Message<string, string> { Key = message.key, Value = message.message, Headers = CreateHeaders(message.headers), Timestamp = new Timestamp(DateTime.Now) });
            }

        }

        private Headers CreateHeaders(IDictionary<string, string> headers)
        {
            var kafkaheaders = new Headers();

            foreach (var keyValuePair in headers)
            {
                kafkaheaders.Add(keyValuePair.Key.ToString(), Encoding.UTF8.GetBytes(keyValuePair.Value.ToString()));
            }

            return kafkaheaders;
        }

        private void HandleDeliveryResult(DeliveryResult<string, string> deliveryResult)
        {
            logger.LogInformation($"--------------------------Delivery Result------------------------------------------");

            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
                throw new MessageNotPersistedException();

            logger.LogInformation($"The Persistence Status of the Message Published is {deliveryResult.Status}");

            logger.LogInformation($"The TopicPartitionOffset to which the Message is Published is {deliveryResult.TopicPartitionOffset}");
        }

        private void errorHandler(IProducer<string, string> arg1, Error arg2)
        {
            logger.LogInformation($"---------------------------Error Handler Log-------------------------------------");

            logger.LogInformation($"IsFatal :{arg2.IsFatal} || Error Reason: {arg2.Reason} and Error Code: {arg2.Code} ");
        }

        private void logHandler(IProducer<string, string> arg1, LogMessage arg2)
        {
            logger.LogInformation($"----------------Kafka Producer Log Handler Message-------------------------------");

            logger.LogInformation($"Message is {arg2.Message} || Critical Level: {arg2.Level} || Name is {arg2.Name} || Facility is {arg2.Facility}");
        }
    }
}
