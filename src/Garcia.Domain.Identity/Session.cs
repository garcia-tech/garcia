namespace Garcia.Domain.Identity
{
    public class Session<TKey> : EntityBase<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey UserId { get; set; }
        public string Token { get; set; }
        public string CreatedByIp { get; set; }
        public string RenewedToken { get; set; }
        public bool Active { get; set; } = true;
        public DateTimeOffset? RenewedOn { get; set; }
        public string RenewedIp { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
