using Confluent.Kafka;
using KafkaInfrastructure.Consumer.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaInfrastructure.Consumer
{
    public class HostedKafkaConsumerProcessor : IHostedKafkaConsumerProcessor
    {
        private readonly ILogger<HostedKafkaConsumerProcessor> logger;
        private readonly IKafkaConsumerConfigFactory kafkaConsumerConfigFactory;
        private readonly IKafkaConsumer kafkaConsumer;

        public HostedKafkaConsumerProcessor(ILogger<HostedKafkaConsumerProcessor> logger,
            IKafkaConsumerConfigFactory kafkaConsumerConfigFactory, IKafkaConsumer kafkaConsumer)
        {
            this.logger = logger;
            this.kafkaConsumerConfigFactory = kafkaConsumerConfigFactory;
            this.kafkaConsumer = kafkaConsumer;
        }

        public async Task BeginKafkaConsumption(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Beginning Consumption from Kafka");

            ConsumerConfig consumerConfig = kafkaConsumerConfigFactory.CreateConsumerConfig();

            kafkaConsumerConfigFactory.LogConfiguration();

            var topics = kafkaConsumerConfigFactory.GetTopics();

            logger.LogInformation($"Consuming from Topics: {JsonConvert.SerializeObject(topics)}");

            await KafkaConsume(consumerConfig, topics, stoppingToken);
        }

        private async Task KafkaConsume(ConsumerConfig consumerConfig, IEnumerable<string> Topic, CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .SetErrorHandler(errorHandler)
                .SetOffsetsCommittedHandler(offsetsCommittedHandler)
                .SetPartitionsAssignedHandler((c, partitions) => { partitionsAssignedHandler(c, partitions); })
                .SetPartitionsRevokedHandler((c, partitions) => { partitionsRevokedHandler(c, partitions); })
                .Build())
            {
                consumer.Subscribe(Topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult.IsPartitionEOF)
                        {
                            logger.LogInformation($"Reached End of Partition {consumeResult.Partition.Value} for Topic {consumeResult.Topic} " +
                                $"with the last offset being {consumeResult.Offset.Value}");

                            continue;
                        }

                        if (kafkaConsumerConfigFactory.IsMessageLogRequired())
                            logger.LogInformation($"Message received from TopicPartitionOffset {consumeResult.TopicPartitionOffset} is {consumeResult.Message.Value.ToHtmlFormat()}");


                        logger.LogInformation($"Message is received from TopicPartitionOffset {consumeResult.TopicPartitionOffset} and MessageLength is {consumeResult.Message.Value.Length}");

                        var KafkaConsumerTask = kafkaConsumer.Consume(new CustomMessage(consumeResult.Topic, consumeResult.Message.Key, consumeResult.Message.Value, CreateHeaders(consumeResult.Message.Headers)));

                        var ConsumerIntervalTask = SetConsumerIntervalTime();

                        var completedTasks = await Task.WhenAny(KafkaConsumerTask, ConsumerIntervalTask);

                        await completedTasks;

                        logger.LogInformation($"Commiting the TopicPartitionOffset {consumeResult.TopicPartitionOffset}");

                        consumer.Commit(consumeResult);
                    }
                    catch (ConsumeException ce)
                    {
                        logger.LogError($"ConsumeException Occurred: {ce.ToString()} and reason : {ce.Error.Reason}");

                        CloseConsumer(consumer);

                        throw;
                    }
                    catch (KafkaException ke)
                    {
                        logger.LogError($"KafkaException Occurred: {ke.ToString()} and reason : {ke.Error.Reason}");

                        CloseConsumer(consumer);

                        throw;
                    }
                    catch (OperationCanceledException oce)
                    {
                        logger.LogError($"Operation has been cancelled, closing consumer");

                        CloseConsumer(consumer);

                        throw;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"GenericException Occurred: {ex.ToString()}");
                    }
                }
            }
        }

        private void CloseConsumer(IConsumer<string, string> consumer)
        {
            if (consumer != null)
            {
                logger.LogInformation($"Closing the consumer and exiting");

                consumer?.Close();
            }
        }

        private async Task SetConsumerIntervalTime()
        {
            var consumerIntervalTime = kafkaConsumerConfigFactory.GetConsumerIntervalTime();

            logger.LogInformation($"Awaiting for the Set Consumer Interval Time of {consumerIntervalTime}");

            await Task.Delay(TimeSpan.FromMilliseconds(consumerIntervalTime));
        }

        private IDictionary<string, string> CreateHeaders(Headers headers)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            var kafkaHeaders = headers.GetEnumerator();

            while (kafkaHeaders.MoveNext())
            {
                dictionary.Add(kafkaHeaders.Current.Key, Encoding.UTF8.GetString(kafkaHeaders.Current.GetValueBytes()));
            }

            return dictionary;
        }

        private void errorHandler(IConsumer<string, string> arg1, Error arg2)
        {
            logger.LogInformation($"---------------------------Error Handler Log-------------------------------------");

            logger.LogInformation($"IsFatal :{arg2.IsFatal} || IsBrokerError:{arg2.IsBrokerError} || Error Reason: {arg2.Reason} and Error Code: {arg2.Code}".ToHtmlFormat());
        }

        private void offsetsCommittedHandler(IConsumer<string, string> arg1, CommittedOffsets arg2)
        {
            logger.LogInformation($"---------------------Offset Handler Log for Commits--------------------------------------------");

            string message = $"The Topic is {arg2?.Offsets.First()?.Topic} and Partition is {arg2?.Offsets.First()?.Partition} and the Offset is {arg2?.Offsets.First()?.Offset}" +
                $"IsFatal: {arg2?.Error.IsFatal} || IsBrokerError: {arg2?.Error.IsBrokerError} || Status: {arg2?.Error.Reason} and Code: {arg2?.Error.Code}";

            logger.LogInformation(message.ToHtmlFormat());
        }

        private void partitionsRevokedHandler(IConsumer<string, string> c, List<TopicPartitionOffset> partitions)
        {
            logger.LogInformation($"---------------------------Revoking Partitions-------------------------------------");

            logger.LogInformation($"Revoking Assignment from Topic {partitions?.First()?.Topic} and  the partitions revoked: [{string.Join(",", partitions)}]".ToHtmlFormat());
        }

        private void partitionsAssignedHandler(IConsumer<string, string> c, List<TopicPartition> partitions)
        {
            logger.LogInformation($"---------------------------Assigning Partitions-------------------------------------");

            logger.LogInformation($"Assigning Partitions for Topic {partitions?.First()?.Topic} and Assigned partitions: [{string.Join(",", partitions)}]".ToHtmlFormat());
        }
    }
}
