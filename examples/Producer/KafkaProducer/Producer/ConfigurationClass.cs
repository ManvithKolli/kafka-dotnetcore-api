using System.Text;

namespace KafkaProducer.Producer
{
    public class ConfigurationClass
    {
        public string ProducerTopic { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"ProducerTopic :{ ProducerTopic}");           

            return builder.ToString();
        }
    }
}
