namespace MDIT.Kafka.Producers
{
    public class IntMessageFactory : MessageFactory<int>
    {
        protected override IMessage<int> CreateCore(int key)
        {
            return new IntMessage(key);
        }

        private class IntMessage : IMessage<int>
        {
            public IntMessage(int key)
            {
                Key = key;
                Content = $"Content: {key}";
            }
            
            public override string ToString()
            {
                return $"{Key}";
            }

            public string Type { get; private set; } = nameof(IntMessage);
            public int Key { get; }
            public string Content { get; private set; }
        }
    }
}