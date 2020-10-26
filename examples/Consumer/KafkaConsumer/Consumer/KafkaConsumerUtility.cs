using KafkaInfrastructure.Consumer.Domain;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KafkaConsumer.Consumer
{
    public class KafkaConsumerUtility : IKafkaConsumer
    {
        private readonly ILogger<KafkaConsumerUtility> logger;

        public KafkaConsumerUtility(ILogger<KafkaConsumerUtility> logger)
        {
            this.logger = logger;
        }


        public async Task Consume(CustomMessage Message)
        {

            logger.LogInformation($"The Message received is {Message.message} and the Key received is {Message.key}");

            foreach (var keyvaluepair in Message.headers)
            {
                logger.LogInformation($"Message Headers  Key is {keyvaluepair.Key} and Value is {keyvaluepair.Value}");
            }
        }
    }
}
