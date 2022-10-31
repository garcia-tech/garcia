namespace Garcia.Infrastructure.Identity
{
    public class RefreshTokenOptions
    {
        public TimeSpan ValidFor { get; set; }
        public DateTime Expiration => DateTime.UtcNow.Add(ValidFor);
    }
}
