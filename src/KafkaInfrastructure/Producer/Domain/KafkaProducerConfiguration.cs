using KafkaInfrastructure.Resiliency.Domain;
using System.Text;

namespace KafkaInfrastructure.Producer.Domain
{
    public class KafkaProducerConfiguration
    {
        public string Acks { get; set; } = "Leader";

        public int? QueueBufferingMaxKbytes { get; set; } = 1048576;

        public int? QueueBufferingMaxMessages { get; set; } = 100000;

        public int? MessageTimeoutMs { get; set; } = 300000;

        public int? RequestTimeoutMs { get; set; } = 10000;

        public double? LingerMs { get; set; } = 0.5;

        public bool? EnableIdempotence { get; set; } = false;

        public int? MessageSendMaxRetries { get; set; } = 2;

        public int? RetryBackoffMs { get; set; } = 100;

        public int? BatchNumMessages { get; set; } = 10000;

        public string ProducerResilientPolicy { get; set; } = ResiliencyPolicyNames.KafkaProduceRetryPolicy.ToString();

        public bool IsProducerResilienceRequired { get; set; } = false;

        const string separator = " => ";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"Acks :{Acks} {separator}");
            builder.Append($"QueueBufferingMaxKbytes :{QueueBufferingMaxKbytes} {separator}");
            builder.Append($"QueueBufferingMaxMessages :{QueueBufferingMaxMessages} {separator}");
            builder.Append($"MessageTimeoutMs :{MessageTimeoutMs} {separator}");
            builder.Append($"RequestTimeoutMs :{RequestTimeoutMs} {separator}");
            builder.Append($"LingerMs :{LingerMs} {separator}");
            builder.Append($"EnableIdempotence :{EnableIdempotence} {separator}");
            builder.Append($"MessageSendMaxRetries :{MessageSendMaxRetries} {separator}");
            builder.Append($"RetryBackoffMs :{RetryBackoffMs} {separator}");
            builder.Append($"BatchNumMessages :{BatchNumMessages} {separator}");
            builder.Append($"ResilientPolicy :{ProducerResilientPolicy} {separator}");
            builder.Append($"IsProducerResilienceRequired :{IsProducerResilienceRequired}");
            return builder.ToString();
        }
    }
}
