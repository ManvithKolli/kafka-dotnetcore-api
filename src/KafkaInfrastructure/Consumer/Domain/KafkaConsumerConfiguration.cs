using KafkaInfrastructure.Resiliency.Domain;
using Newtonsoft.Json;
using System.Text;

namespace KafkaInfrastructure.Consumer.Domain
{
    public class KafkaConsumerConfiguration
    {
        public Topics[] Topics { get; set; }

        public string GroupId { get; set; }

        public string AutoOffsetReset { get; set; } = "Latest";

        public string IsolationLevel { get; set; } = "ReadUncommitted";

        public bool? EnableAutoCommit { get; set; } = false;

        public int? AutoCommitIntervalMs { get; set; } = 5000;

        public bool? EnableAutoOffsetStore { get; set; } = false;

        public bool? EnablePartitionEof { get; set; } = true;

        public bool? CheckCrcs { get; set; } = true;

        public int? MaxPollIntervalMs { get; set; } = 300000;

        public int? HeartbeatIntervalMs { get; set; } = 9000;

        public int? SessionTimeoutMs { get; set; } = 27000;

        public int? FetchMinBytes { get; set; } = 1;

        public int? FetchMaxBytes { get; set; } = 52428800;

        public int? FetchWaitMaxMs { get; set; } = 500;

        public int? MaxPartitionFetchBytes { get; set; } = 1048576;

        public int? ReconnectBackoffMs { get; set; } = 100;

        public int? ReconnectBackoffMaxMs { get; set; } = 10000;

        public string ConsumerResilientPolicy { get; set; } = ResiliencyPolicyNames.KafkaConsumeRetryPolicy.ToString();

        public bool IsMessageLogRequired { get; set; } = true;

        public double ConsumerIntervalTimeMilliSeconds { get; set; } = 5000; // MaxLimit= MaxPollIntervalMs-MaxPollBufferTimeMilliSeconds

        public double MaxPollBufferTimeMilliSeconds { get; set; } = 60000;

        public bool IsConsumerResilienceRequired { get; set; } = true;

        const string separator = " => ";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"Topics:{JsonConvert.SerializeObject(Topics)} {separator}");
            builder.Append($"GroupId :{GroupId} {separator}");
            builder.Append($"AutoOffsetReset :{AutoOffsetReset} {separator}");
            builder.Append($"IsolationLevel :{IsolationLevel} {separator}");
            builder.Append($"EnableAutoCommit :{EnableAutoCommit} {separator}");
            builder.Append($"AutoCommitIntervalMs :{AutoCommitIntervalMs} {separator}");
            builder.Append($"EnableAutoOffsetStore :{EnableAutoOffsetStore} {separator}");
            builder.Append($"EnablePartitionEof :{EnablePartitionEof} {separator}");
            builder.Append($"CheckCrcs :{CheckCrcs} {separator}");
            builder.Append($"MaxPollIntervalMs :{MaxPollIntervalMs} {separator}");
            builder.Append($"HeartbeatIntervalMs :{HeartbeatIntervalMs} {separator}");
            builder.Append($"SessionTimeoutMs :{SessionTimeoutMs} {separator}");
            builder.Append($"FetchMinBytes :{FetchMinBytes} {separator}");
            builder.Append($"FetchMaxBytes :{FetchMaxBytes} {separator}");
            builder.Append($"FetchWaitMaxMs :{FetchWaitMaxMs} {separator}");
            builder.Append($"MaxPartitionFetchBytes :{MaxPartitionFetchBytes} {separator}");
            builder.Append($"ReconnectBackoffMs :{ReconnectBackoffMs} {separator}");
            builder.Append($"ReconnectBackoffMaxMs :{ReconnectBackoffMaxMs} {separator}");
            builder.Append($"ConsumerResilientPolicy :{ConsumerResilientPolicy} {separator}");
            builder.Append($"IsMessageLogRequired :{IsMessageLogRequired} {separator}");
            builder.Append($"IsConsumerResilienceRequired :{IsConsumerResilienceRequired} {separator}");
            builder.Append($"ConsumerIntervalTimeMilliSeconds :{ConsumerIntervalTimeMilliSeconds} {separator}");
            builder.Append($"MaxPollBufferTimeMilliSeconds :{MaxPollBufferTimeMilliSeconds}");
            return builder.ToString();
        }
    }

    public class Topics
    {
        public string TopicName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
