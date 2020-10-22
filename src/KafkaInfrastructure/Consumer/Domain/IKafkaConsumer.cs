using System.Threading.Tasks;

namespace KafkaInfrastructure.Consumer.Domain
{
    public interface IKafkaConsumer
    {
        Task Consume(CustomMessage Message);
    }
}
