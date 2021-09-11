using System;
using Confluent.Kafka;

namespace MDIT.Kafka.Producers
{
    public interface IMessageSerializer<TKey> : ISerializer<Message<TKey>>
    {
    }

    public class MessageSerializer<TKey> : IMessageSerializer<TKey>
    {
        public byte[] Serialize(Message<TKey> data, SerializationContext context)
        {
            return new byte[8];
        }
    }

    public class MessageDeserializer<TKey> : IDeserializer<Message<TKey>>
    {
        public Message<TKey> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            throw new NotImplementedException();
        }
    }
}