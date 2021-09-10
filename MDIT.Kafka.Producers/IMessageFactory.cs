using Confluent.Kafka;

namespace MDIT.Kafka.Producers
{
    public interface IMessageFactory<TKey>
    {
        Message<TKey, IMessage<TKey>> Create(
            TKey key,
            Timestamp? timestamp = null,
            Headers headers = null);
    }
}