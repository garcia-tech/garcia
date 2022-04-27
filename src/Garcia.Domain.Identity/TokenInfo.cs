namespace Garcia.Domain.Identity
{
    public class TokenInfo : ValueObject
    {
        public string Id { get; set; }
        public string AuthToken { get; set; }
        public int ExpiresIn { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return AuthToken;
            yield return ExpiresIn;
        }
    }
}
