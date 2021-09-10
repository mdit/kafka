using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MDIT.Kafka.Producers
{
    public class MessageProducer<TKey> : IDisposable
    {
        private readonly IKeyFactory<TKey> _keyFactory;
        private readonly IMessageFactory<TKey> _messageFactory;
        private readonly ILogger<MessageProducer<TKey>> _logger;
        private readonly IProducer<TKey, Message<TKey>> _producer;

        public MessageProducer(
            ILogger<MessageProducer<TKey>> logger,
            IKeyFactory<TKey> keyFactory,
            IMessageFactory<TKey> messageFactory,
            IAsyncSerializer<Message<TKey>> serializer,
            IOptions<ProducerConfig> producerConfigAccessor)
        {
            _logger = logger;
            _keyFactory = keyFactory;
            _messageFactory = messageFactory;

            var producerConfig = producerConfigAccessor.Value;
            
            var settings = string.Join(Environment.NewLine, producerConfig.Select(x => $"{x.Key}: {x.Value}"));
            _logger.LogInformation($"{settings}");
            var producerBuilder = new ProducerBuilder<TKey, Message<TKey>>(producerConfig);
            producerBuilder.SetValueSerializer(serializer);
            _producer = producerBuilder.Build();
        }
        
        public async Task Produce(string topic, int messageCount, CancellationToken cancellationToken)
        {
            var i = 0;
            while (i < messageCount && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = _messageFactory.Create(_keyFactory.Create(i));
                    var deliveryResult =
                        await _producer.ProduceAsync(topic, message, cancellationToken)
                                       .ConfigureAwait(false);
                    
                    _logger.LogDebug("Message delivered {message}", deliveryResult.Message);
                }
                catch (ProduceException<TKey, IMessage<TKey>> exception)
                {
                    _logger.LogError("Could not deliver message {message}", exception.Message);
                }

                i++;
            }
        }

        public void Dispose()
        {
            _producer.Flush();
            _producer?.Dispose();
        }
    }
}