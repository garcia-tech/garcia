using System.ComponentModel.DataAnnotations.Schema;
using Garcia.Domain;
using Garcia.Domain.Identity;

public class User : Entity<long>, IUserEntity<long>
{
    public string Username { get; set; }
    public string Password { get; set; }

    [NotMapped]
    public List<string> Roles { get; set; } = new List<string>();
}

