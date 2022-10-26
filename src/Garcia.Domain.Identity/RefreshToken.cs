namespace Garcia.Domain.Identity
{
    public class RefreshToken
    {
        public RefreshToken()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public string Token { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByIp { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset? RevokedDate { get; set; }
        public bool Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
    }
}
