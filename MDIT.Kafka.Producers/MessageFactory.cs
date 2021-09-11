using Confluent.Kafka;

namespace MDIT.Kafka.Producers
{
    public abstract class MessageFactory<TKey> : IMessageFactory<TKey>
    {
        public Message<TKey, Message> Create(TKey key, Timestamp? timestamp = null, Headers headers = null)
        {
            var message = CreateCore(key);
            return new Message<TKey, Message> {
                Key = key,
                Value = message,
                Timestamp = timestamp ?? Timestamp.Default,
                Headers = headers
            };
        }

        protected abstract Message<TKey> CreateCore(TKey key);
    }
}