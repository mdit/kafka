using Confluent.Kafka;

namespace MDIT.Kafka.Producers
{
    public abstract class MessageFactory<TKey> : IMessageFactory<TKey>
    {
        public Message<TKey, IMessage<TKey>> Create(TKey key, Timestamp? timestamp = null, Headers headers = null)
        {
            var message = CreateCore(key);
            return new Message<TKey, IMessage<TKey>> {
                Key = key,
                Value = message,
                Timestamp = timestamp ?? Timestamp.Default,
                Headers = headers
            };
        }

        protected abstract IMessage<TKey> CreateCore(TKey key);
    }
}