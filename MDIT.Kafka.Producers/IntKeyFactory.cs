namespace MDIT.Kafka.Producers
{
    public class IntKeyFactory : IKeyFactory<int>
    {
        public int Create(int seed) => seed;
    }
}