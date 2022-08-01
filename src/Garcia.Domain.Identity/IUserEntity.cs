namespace Garcia.Domain.Identity
{
    public interface IUserEntity<TKey> : IEntity<TKey>
    { 
        string Username { get; set; }
        string Password { get; set; }
        public List<string> Roles { get; set; }
            
    }
}
