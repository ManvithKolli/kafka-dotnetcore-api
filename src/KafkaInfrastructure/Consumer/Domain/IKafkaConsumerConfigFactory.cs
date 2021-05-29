using Confluent.Kafka;
using System.Collections.Generic;

namespace KafkaInfrastructure.Consumer.Domain
{
    public interface IKafkaConsumerConfigFactory
    {
        ConsumerConfig CreateConsumerConfig();

        IEnumerable<string> GetTopics();

        string GetResilientPolicy();

        bool IsMessageLogRequired();

        double GetConsumerIntervalTime();

        void LogConfiguration();
    }
}
