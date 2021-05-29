using KafkaInfrastructure.Consumer;
using KafkaInfrastructure.Consumer.Domain;
using KafkaInfrastructure.Consumer.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KafkaConsumerExtension
    {
        public static IServiceCollection IncludeKafkaConsumer(this IServiceCollection services, KafkaServerConfiguration kafkaServerConfiguration,
            KafkaConsumerConfiguration kafkaConsumerConfiguration)
        {
            return services.AddKafkaConsumerConfiguration(kafkaServerConfiguration, kafkaConsumerConfiguration)
                .AddKafkaConsumerDependentServices();

        }

        public static IServiceCollection AddKafkaConsumerDependentServices(this IServiceCollection services)
        {
            services.AddSingleton<IHostedKafkaConsumerProcessor, HostedKafkaConsumerProcessor>();
            services.AddSingleton<IKafkaConsumerConfigFactory, KafkaConsumerConfigFactory>();
            services.AddSingleton<IKafkaResilientConsumer, KafkaResilientConsumer>();

            services.AddHostedService<KafkaConsumerHostedService>();

            return services;
        }

        public static IServiceCollection AddKafkaConsumerConfiguration(this IServiceCollection services, KafkaServerConfiguration kafkaServerConfiguration,
            KafkaConsumerConfiguration kafkaConsumerConfiguration)
        {
            services.AddSingleton(kafkaServerConfiguration);
            services.AddSingleton(kafkaConsumerConfiguration);
            return services;
        }
    }
}