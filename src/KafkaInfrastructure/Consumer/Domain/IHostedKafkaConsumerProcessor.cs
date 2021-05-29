using System.Threading;
using System.Threading.Tasks;

namespace KafkaInfrastructure.Consumer.Domain
{
    public interface IHostedKafkaConsumerProcessor
    {
        Task BeginKafkaConsumption(CancellationToken stoppingToken);
    }
}
