using Confluent.Kafka;
using System.Text;

namespace KafkaInfrastructure.Producer.Domain
{
    public class KafkaServerConfiguration
    {
        public string BootstrapServers { get; set; }

        public SaslMechanism SaslMechanism { get; set; } = SaslMechanism.Gssapi;

        public SecurityProtocol SecurityProtocol { get; set; } = SecurityProtocol.SaslSsl;

        public string SslCaLocation { get; set; }

        public string SaslKerberosKeytab { get; set; }

        public string SaslKerberosPrincipal { get; set; }

        public string SaslKerberosServiceName { get; set; }

        public string SaslKerberosKinitCmd { get; set; }

        const string separator = " => ";

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"BootstrapServers :{ BootstrapServers} {separator}");
            builder.Append($"SaslMechanism :{ SaslMechanism} {separator}");
            builder.Append($"SecurityProtocol :{ SecurityProtocol} {separator}");
            builder.Append($"SslCaLocation :{ SslCaLocation} {separator}");
            builder.Append($"SaslKerberosKeytab :{ SaslKerberosKeytab} {separator}");
            builder.Append($"SaslKerberosPrincipal :{ SaslKerberosPrincipal} {separator}");
            builder.Append($"SaslKerberosServiceName :{ SaslKerberosServiceName} {separator}");
            builder.Append($"SaslKerberosKinitCmd :{ SaslKerberosKinitCmd}");

            return builder.ToString();
        }
    }
}
