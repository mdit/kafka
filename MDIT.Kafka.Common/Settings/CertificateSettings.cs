using System;

namespace MDIT.Kafka.Settings
{
    /// <summary>
    /// Settings for SSL Certificates.
    /// </summary>
    /// <param name="CertificateFolder">The folder containing the ssl certificate.</param>
    public record CertificateSettings(string CertificateFolder);
}