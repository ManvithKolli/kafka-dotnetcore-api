using System.Collections.Generic;

namespace KafkaInfrastructure.Producer.Domain
{
    public class CustomMessage
    {
        public CustomMessage(string source, string message) : this(source, string.Empty, message, new Dictionary<string, string>())
        {

        }

        public CustomMessage(string source, string key, string message, IDictionary<string, string> headers)
        {
            this.source = source;
            this.key = key;
            this.message = message;
            this.headers = headers;
        }

        //TODO: For extending create more constructors for other purposes

        public string source { get; set; }
        public string key { get; set; }
        public string message { get; set; }
        public IDictionary<string, string> headers { get; set; }
    }
}
