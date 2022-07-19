using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Garcia.Domain.Identity
{
    public class User<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    { 
        public string Username { get; set; }
        public string Password { get; set; }

        [BsonIgnore]
        [NotMapped]
        public virtual List<string> Roles { get; set; } = new List<string>();
            
    }
}
