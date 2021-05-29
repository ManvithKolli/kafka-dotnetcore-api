using System.Text;

namespace KafkaInfrastructure.Resiliency.Domain
{
    public class ResiliencyConfiguration
    {

        public int messagingKafkaRetryCount { get; set; } = 5;

        public int messagingKafkaConsumeRetryTimeSpan { get; set; } = 5;

        const string separator = " => ";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"MessagingKafkaRetryCount :{messagingKafkaRetryCount} {separator}");
            builder.Append($"messagingKafkaRetryTimeSpan :{messagingKafkaConsumeRetryTimeSpan} {separator}");
           
            return builder.ToString();
        }
    }
}
