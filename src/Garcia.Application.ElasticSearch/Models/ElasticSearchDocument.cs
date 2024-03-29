﻿using Nest;

namespace Garcia.Application.ElasticSearch.Models
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
