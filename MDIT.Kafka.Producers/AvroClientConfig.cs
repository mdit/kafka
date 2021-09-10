using System.Collections.Generic;

namespace MDIT.Kafka.Producers
{
    public class AvroClientConfig : Dictionary<string, string>
    {
        public AvroClientConfig()
        {
            this["ssl.endpoint.identification.algorithm"] = "https";
            this["security.protocol"] = "SASL_SSL";
            this["sasl.mechanism"] = "PLAIN";
            this["bootstrap.servers"] = "<BROKER ENDPOINT>";
            this["sasl.jaas.config"] = "org.apache.kafka.common.security.plain.PlainLoginModule required username='<API KEY>' password='<API SECRET>'";
            this["basic.auth.credentials.source"] = "USER_INFO";
            this["schema.registry.basic.auth.user.info"] = "<SR API KEY>:<SR API SECRET>";
            this["schema.registry.url"] = "https://<SR ENDPOINT>";
        }
    }
}