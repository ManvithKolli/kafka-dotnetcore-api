using Microsoft.Extensions.Configuration;

namespace KafkaConsumer.Utility
{
    public static class ConfigurationUtility
    {
        public static T GetConfiguration<T>(IConfiguration configuration, string sectionName) where T : class, new()
        {
            var myOptions = new T();

            configuration.Bind(sectionName, myOptions);

            return myOptions;
        }
    }
}
