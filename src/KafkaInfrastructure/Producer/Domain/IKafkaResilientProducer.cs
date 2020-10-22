using System.Threading.Tasks;

namespace KafkaInfrastructure.Producer.Domain
{
    public interface IKafkaResilientProducer
    {
        Task<DeliveryResult> Produce(CustomMessage message);
    }
}
