using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Domain;
using Garcia.Domain.Identity;

namespace Garcia.Application.Tests
{
    public class TestUser : Entity<long>, IUserEntity<long>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
