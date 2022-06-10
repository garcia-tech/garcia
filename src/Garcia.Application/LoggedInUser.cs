using System.Collections.Generic;
using Garcia.Application.Contracts.Identity;

namespace Garcia.Application
{
    public class LoggedInUser : IUser
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
