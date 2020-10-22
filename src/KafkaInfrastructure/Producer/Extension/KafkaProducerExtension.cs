using KafkaInfrastructure.Producer;
using KafkaInfrastructure.Producer.Domain;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KafkaProducerExtension
    {
        public static IServiceCollection IncludeKafkaPublisher(this IServiceCollection services, KafkaServerConfiguration kafkaServerConfiguration,
            KafkaProducerConfiguration kafkaProducerConfiguration)
        {
            return services.AddKafkaProducerDependentServices()
                .AddKafkaProducerConfiguration(kafkaServerConfiguration, kafkaProducerConfiguration);

        }

        public static IServiceCollection AddKafkaProducerDependentServices(this IServiceCollection services)
        {
            services.AddTransient<IKafkaProducer, KafkaProducer>();
            services.AddTransient<IKafkaResilientProducer, KafkaResilientProducer>();
            services.AddTransient<IKafkaProducerConfigFactory, KafkaProducerConfigFactory>();
            services.AddTransient<IDeliveryResultAssembler, DeliveryResultAssembler>();
            return services;
        }

        public static IServiceCollection AddKafkaProducerConfiguration(this IServiceCollection services, KafkaServerConfiguration kafkaServerConfiguration,
            KafkaProducerConfiguration kafkaProducerConfiguration)
        {
            services.AddSingleton(kafkaServerConfiguration);
            services.AddSingleton(kafkaProducerConfiguration);
            return services;
        }
    }
}