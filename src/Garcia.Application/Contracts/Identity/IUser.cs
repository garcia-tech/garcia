﻿using System.Collections.Generic;

namespace Garcia.Application.Contracts.Identity
{
    public interface IUser
    {
        string Username { get; set; }
        string Id { get; set; }
        List<string> Roles { get; set; }
    }
}
