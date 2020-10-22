using KafkaInfrastructure.Resiliency.KafkaResiliencyPolicies;
using System.Collections.Generic;

namespace KafkaInfrastructure.Resiliency.Domain
{
    public static class MessagingPoliciesAggregator
    {
        public static IEnumerable<IAsyncResiliencyPolicyFactory> AggregateKafkaPolicies(ResiliencyConfiguration messagingResiliencyConfiguration)
        {
            return new List<IAsyncResiliencyPolicyFactory>()
            {
               new KafkaProduceRetryPolicy(messagingResiliencyConfiguration),
               new KafkaConsumeRetryPolicy(messagingResiliencyConfiguration)
            };
        }

    }
}
