using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Domain.Identity
{
    public class Session<TKey, TUserId> : Entity<TKey>
        where TKey : IEquatable<TKey>
        where TUserId : IEquatable<TUserId>
    {
        public TUserId UserId { get; set; }
        public string Token { get; set; }
        public string CreatedByIp { get; set; }
        public string RenewedToken { get; set; }
        public bool Active { get; set; } = true;
        public DateTimeOffset? RenewedOn { get; set; }
        public string RenewedIp { get; set; }
    }
}
