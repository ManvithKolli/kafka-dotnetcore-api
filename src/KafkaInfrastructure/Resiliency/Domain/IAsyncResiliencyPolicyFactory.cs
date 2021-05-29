using Polly;

namespace KafkaInfrastructure.Resiliency.Domain
{
    public interface IAsyncResiliencyPolicyFactory
    {
        AsyncPolicy CreateResilientPolicy();
    }
}
