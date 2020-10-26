using KafkaInfrastructure.Producer.Domain;
using KafkaInfrastructure.Resiliency.Domain;
using KafkaProducer.Producer;
using KafkaProducer.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace KafkaProducer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly ILogger<Startup> logger;

        public KafkaServerConfiguration kafkaServerConfiguration { get; set; }

        public KafkaProducerConfiguration kafkaProducerConfiguration { get; set; }

        public ResiliencyConfiguration resiliencyConfiguration { get; set; }

        public ConfigurationClass configurationClass { get; set; }

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "kafkaProducer", Version = "v1" });
                c.CustomSchemaIds((type) => type.FullName);
            });

            kafkaServerConfiguration = ConfigurationUtility.GetConfiguration<KafkaServerConfiguration>(Configuration, "common:kafka");

            logger.LogInformation($"Kafka Server Configuration: {kafkaServerConfiguration}");

            kafkaProducerConfiguration = ConfigurationUtility.GetConfiguration<KafkaProducerConfiguration>(Configuration, "kafkaProducer");

            logger.LogInformation($"Kafka Producer Configuration :{kafkaProducerConfiguration}");

            resiliencyConfiguration = ConfigurationUtility.GetConfiguration<ResiliencyConfiguration>(Configuration, "kafkaProducer");

            configurationClass= ConfigurationUtility.GetConfiguration<ConfigurationClass>(Configuration, "kafkaProducer");

            logger.LogInformation($"Configuration :{configurationClass}");

            services.AddCustomResiliency(resiliencyConfiguration);

            services.Configure<ConfigurationClass>(Configuration.GetSection("kafkaProducer"));

            services.IncludeKafkaPublisher(kafkaServerConfiguration, kafkaProducerConfiguration);
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", string.Concat("kafkaProducer", " ", "v1"));
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
