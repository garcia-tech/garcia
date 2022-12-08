using Garcia.Application.Contracts.Identity;

public class UserDto : IUser
{
    public string Id { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}


