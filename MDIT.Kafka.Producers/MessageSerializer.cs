using System;
using Confluent.Kafka;

namespace MDIT.Kafka.Producers
{
    public interface IMessageSerializer<TKey> : ISerializer<IMessage<TKey>>
    {
    }

    public class MessageSerializer<TKey> : ISerializer<>
    {
        public byte[] Serialize(IMessage<TKey> data, SerializationContext context)
        {
            return new byte[8];
        }
    }

    public class MessageDeserializer<TKey> : IDeserializer<IMessage<TKey>>
    {
        public IMessage<TKey> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            throw new NotImplementedException();
        }
    }
}