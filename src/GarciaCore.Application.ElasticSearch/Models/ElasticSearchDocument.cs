using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace GarciaCore.Application.ElasticSearch.Models
{
    public abstract class ElasticSearchDocument<TKey> : IElasticSearchDocument<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public CompletionField Suggest { get; set; }
        public double? Score { get; set; }
        public string SearchField { get; set; }
        public DateTimeOffset CreatedOn { get; } = DateTime.UtcNow;
    }
}
