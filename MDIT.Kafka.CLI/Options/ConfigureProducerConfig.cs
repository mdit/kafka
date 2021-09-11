// Copyright (c) AMR GP Limited 2021.

using System.IO;
using System.Reflection;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace MDIT.Kafka.CLI.Options
{
    /// <summary>
    /// Configures a <see cref="ProducerConfig"/> with the location of the Confluent ca certificate.
    /// </summary>
    internal class ConfigureProducerConfig : IConfigureOptions<ProducerConfig>
    {
        public void Configure(ProducerConfig producerConfig)
        {
            producerConfig.SslCaLocation = GetSslCertificateLocation();
        }

        private static string GetSslCertificateLocation()
        {
            return Path.Combine(
                Path.GetDirectoryName(Assembly.GetCallingAssembly().Location),
                "Certificates",
                "cacert.pem");
        }
    }
}
