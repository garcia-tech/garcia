using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Identity;

namespace Garcia.Application.Tests
{
    public class TestUserDto : IUser
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
