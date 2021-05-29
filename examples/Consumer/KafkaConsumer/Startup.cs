using KafkaConsumer.Utility;
using KafkaInfrastructure.Consumer.Domain;
using KafkaInfrastructure.Resiliency.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KafkaConsumer.Consumer;

namespace KafkaConsumer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly ILogger<Startup> logger;

        public KafkaServerConfiguration kafkaServerConfiguration { get; set; }

        public KafkaConsumerConfiguration kafkaConsumerConfiguration { get; set; }

        public ResiliencyConfiguration resiliencyConfiguration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            logger = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Default", LogLevel.Information);
                builder.AddConsole();

            }).CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            resiliencyConfiguration = ConfigurationUtility.GetConfiguration<ResiliencyConfiguration>(Configuration, "kafkaConsumer");

            kafkaServerConfiguration = ConfigurationUtility.GetConfiguration<KafkaServerConfiguration>(Configuration, "common:kafka");

            logger.LogInformation($"Kafka Server Configuration: {kafkaServerConfiguration}");

            kafkaConsumerConfiguration = ConfigurationUtility.GetConfiguration<KafkaConsumerConfiguration>(Configuration, "kafkaConsumer");

            logger.LogInformation($"Kafka Consumer Configuration: {kafkaConsumerConfiguration}");

            services.AddCustomResiliency(resiliencyConfiguration);

            services.AddSingleton<IKafkaConsumer, KafkaConsumerUtility>();

            services.IncludeKafkaConsumer(kafkaServerConfiguration, kafkaConsumerConfiguration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
