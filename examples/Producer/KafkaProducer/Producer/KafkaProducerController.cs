using KafkaInfrastructure.Producer.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KafkaProducer.Producer
{
    public class KafkaProducerController : ControllerBase
    {
        private readonly ILogger<KafkaProducerController> logger;
        private readonly ConfigurationClass configuration;
        private readonly IKafkaResilientProducer kafkaProducer;

        public KafkaProducerController(ILogger<KafkaProducerController> logger, IOptionsSnapshot<ConfigurationClass> configuration ,
            IKafkaResilientProducer kafkaProducer)
        {
            this.logger = logger;
            this.configuration = configuration.Value;
            this.kafkaProducer = kafkaProducer;
        }

        [HttpPost("/publish")]
        public async Task<IActionResult> Publish([FromBody] KafkaProducerRequest kafkaProducerRequest)
        {
            logger.LogInformation("Started Publishing into Kafka Topic");

            DeliveryResult deliveryResult =
                await kafkaProducer.Produce(new CustomMessage(configuration.ProducerTopic, kafkaProducerRequest.Message));

            logger.LogInformation($"Delivery Result:{deliveryResult}");

            return Ok();
        }
    }
}
