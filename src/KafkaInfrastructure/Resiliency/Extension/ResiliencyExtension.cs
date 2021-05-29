using System.Collections.Generic;
using KafkaInfrastructure.Resiliency.Domain;
using Polly.Registry;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ResiliencyExtension
    {
        public static IServiceCollection AddCustomResiliency(this IServiceCollection services, ResiliencyConfiguration resiliencyConfiguration)
        {
            AddResilientPoliciesToRegistry(services, resiliencyConfiguration);
            return services;
        }

        private static void AddResilientPoliciesToRegistry(IServiceCollection services, ResiliencyConfiguration resiliencyConfiguration)
        {
            var Registry = services.AddPolicyRegistry(); // should be added only once

            AddMessagingResilientPolicies(resiliencyConfiguration, Registry);

        }

        private static void AddMessagingResilientPolicies(ResiliencyConfiguration resiliencyConfiguration, IPolicyRegistry<string> registry)
        {
            AddAsyncPolicies(MessagingPoliciesAggregator.AggregateKafkaPolicies(resiliencyConfiguration), registry);
        }

        private static void AddAsyncPolicies(IEnumerable<IAsyncResiliencyPolicyFactory> retryPolicies, IPolicyRegistry<string> registry)
        {
            foreach (var policies in retryPolicies)
            {
                if (!registry.ContainsKey(policies.GetType().Name))
                {
                    registry.Add(policies.GetType().Name, policies.CreateResilientPolicy());
                }
            }
        }
    }
}
