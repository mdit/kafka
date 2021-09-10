namespace MDIT.Kafka.Producers
{
    public abstract class Message
    {
        string Type { get; }
        string Content { get; }
    }

    public abstract class Message<TKey> : Message
    {
        TKey Key { get; }
    }
}