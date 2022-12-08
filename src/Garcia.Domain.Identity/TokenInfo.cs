namespace Garcia.Domain.Identity
{
    public class TokenInfo : ValueObject
    {
        /// <summary>
        /// Requesting user id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Json Web Token.
        /// </summary>
        public string AuthToken { get; set; }
        /// <summary>
        /// Json Web Token expiration.
        /// </summary>
        public int ExpiresIn { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return AuthToken;
            yield return ExpiresIn;
        }
    }
}
