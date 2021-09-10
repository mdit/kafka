using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using MDIT.Kafka.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;

try
{
    var mode = new SelectionPrompt<string>().AddChoices("Producer", "Consumer").Show(AnsiConsole.Console);
    var topic = AnsiConsole.Ask<string>("Topic", "test-topic");
    var messageCount = AnsiConsole.Ask<int>("Message count", 10);

    var services = CreateServices(GetConfiguration()).BuildServiceProvider();

    switch (mode)
    {
        case "Producer":
        {
            using var producer = services.GetService<MessageProducer<int>>();
            await producer.Produce(topic, messageCount, CancellationToken.None);
            break;
        }
        case "Consumer":
            // services.GetService<ExampleMessageConsumer>()
            //         .Run(args);
            break;
    }
}
catch (Exception exception)
{
    AnsiConsole.WriteException(exception);
}
finally
{
    System.Console.ReadLine();
}


IServiceCollection CreateServices(IConfiguration configuration)
{
    return new ServiceCollection().AddLogging(builder => builder.AddConsole())
                                  .AddOptions()
                                  .Configure<ProducerConfig>(options => ConfigureProducerConfig(configuration, options))
                                  .Configure<AvroSerializerConfig>(options => ConfigureAvroSerializerConfig(configuration, options))
                                  .AddAvroClient()
                                  .AddMessageSerializers()
                                  .AddMessageProducer();
}

void ConfigureProducerConfig(IConfiguration config, ProducerConfig producerConfig)
{
    config.GetSection(nameof(ProducerConfig)).Bind(producerConfig);

    producerConfig.SslCaLocation = GetSslCertificateLocation();
}

void ConfigureAvroSerializerConfig(IConfiguration config, AvroSerializerConfig avroSerializerConfig)
{
    config.GetSection(nameof(AvroSerializerConfig)).Bind(avroSerializerConfig);
}

ProducerConfig GetConfig()
{
    return new ProducerConfig
    {
        BootstrapServers = "pkc-41mxj.uksouth.azure.confluent.cloud:9092",
        SecurityProtocol = SecurityProtocol.SaslSsl,
        SaslMechanism = SaslMechanism.Plain,
        SaslUsername = "CYL5OUWVPVJYMNZB",
        SaslPassword = "S6IUekNIG7UqB5/ppRwTmviKCzLfUrWL/0T979SEgb4jh0LQDeTGTqByHMDwe4tr",
       // SslCaLocation = GetSslCertificateLocation()
    };
}

string GetSslCertificateLocation()
{
    return Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        "Certificates",
        "cacert.pem");
}

IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false)
           .Build();
}