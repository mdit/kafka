using System;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;

namespace MDIT.Kafka.Producers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAvroClient(
            this IServiceCollection services,
            AvroClientConfig avroClientConfig,
            AvroSerializerConfig avroSerializerConfig)
        {
            services.AddSingleton<ISchemaRegistryClient, CachedSchemaRegistryClient>();

            var client = new CachedSchemaRegistryClient(avroClientConfig);
            var serializer = new AvroSerializer<Message>(client, avroSerializerConfig);
            services.AddSingleton<ISchemaRegistryClient>(client);
            services.AddSingleton<IAsyncSerializer<Message>>(serializer);
            return services;
        }

        public static IServiceCollection AddMessageProducer(this IServiceCollection services)
        {
            return services.AddScoped<IKeyFactory<int>, IntKeyFactory>()
                           .AddScoped<IMessageFactory<int>, IntMessageFactory>()
                           .AddScoped<MessageProducer<int>>();
        }
    }
}