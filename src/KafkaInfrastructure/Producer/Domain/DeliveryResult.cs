using System.Text;

namespace KafkaInfrastructure.Producer.Domain
{
    public class DeliveryResult
    {
        public string topic { get; set; }

        public int partition { get; set; }

        public long offset { get; set; }

        public string persistenceStatus { get; set; }

        const string separator = " => ";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"topic :{topic} {separator}");
            builder.Append($"partition :{partition} {separator}");
            builder.Append($"offset :{offset} {separator}");
            builder.Append($"persistenceStatus :{persistenceStatus} {separator}");

            return builder.ToString();
        }
    }
}
