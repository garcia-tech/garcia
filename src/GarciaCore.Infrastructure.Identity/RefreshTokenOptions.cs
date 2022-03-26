using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.Identity
{
    public class RefreshTokenOptions
    {
        public TimeSpan ValidFor { get; set; }
        public DateTime Expiration => DateTime.UtcNow.Add(ValidFor);
    }
}
