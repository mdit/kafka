namespace MDIT.Kafka.Producers
{
    public interface IKeyFactory<TKey>
    {
        TKey Create(int seed);
    }
}