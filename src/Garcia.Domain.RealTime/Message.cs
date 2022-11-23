namespace Garcia.Domain.RealTime
{
    public class Message<TKey> : Entity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Group { get; set; }
        public MessageType Type { get; set; }
    }
}