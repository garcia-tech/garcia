namespace Garcia.Domain.Identity
{
    public class RefreshToken : ValueObject
    {
        public RefreshToken()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public string Token { get; set; }
        public DateTimeOffset CreatedDate{ get; set; }
        public string CreatedByIp { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset? RevokedDate { get; set; }
        public bool Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Token;
            yield return CreatedDate;
            yield return CreatedByIp;
            yield return ExpirationDate;
            yield return RevokedDate;
            yield return Revoked;
            yield return RevokedByIp;
            yield return ReplacedByToken;
        }
    }
}
