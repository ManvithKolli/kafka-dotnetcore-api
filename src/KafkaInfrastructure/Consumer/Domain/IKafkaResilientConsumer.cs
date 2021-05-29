using System.Threading;
using System.Threading.Tasks;

namespace KafkaInfrastructure.Consumer.Domain
{
    public interface IKafkaResilientConsumer
    {
        Task Execute(CancellationToken stoppingToken);
    }
}
