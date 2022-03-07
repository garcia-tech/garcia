using System.Collections.Generic;

namespace GarciaCore.Application.Contracts.Identity
{
    public interface IUser
    {
        string UserName { get; set; }
        string Id { get; set; }
        List<string> Roles { get; set; }
    }
}
