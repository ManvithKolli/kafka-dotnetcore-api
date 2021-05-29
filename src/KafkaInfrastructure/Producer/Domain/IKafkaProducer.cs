using System.Threading.Tasks;

namespace KafkaInfrastructure.Producer.Domain
{
    public interface IKafkaProducer
    {
        Task<DeliveryResult> Produce(CustomMessage message);
    }
}
