using Nest;

namespace GarciaCore.Application.ElasticSearch.Models
{
    public interface IElasticSearchDocument<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
        CompletionField Suggest { get; set; }
        double? Score { get; set; }
        string SearchField { get; set; }
        DateTimeOffset CreatedOn { get; }
    }
}
