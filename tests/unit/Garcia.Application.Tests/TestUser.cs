using System.Collections.Generic;
using Garcia.Domain;
using Garcia.Domain.Identity;

namespace Garcia.Application.Tests
{
    public class TestUser : Entity<long>, IUserEntity<long>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
