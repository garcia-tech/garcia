using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Application.ElasticSearch.Models;
using Nest;

namespace GarciaCore.Application.ElasticSearch.Contracts
{
    public interface IElasticSearchService<T, TKey>
        where T : ElasticSearchDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<long> SetDocumentAsync(T document);
        Task<ISearchResponse<T>> SearchAsync(Expression<Func<T, object>> field, string searchText, int? skip = null, int? take = null);
        Task<ISearchResponse<T>> SearchAsync(SearchRequest<T> request);
        Task<ISearchResponse<T>> SearchAsync(SearchDescriptor<T> query);
        Task<ISearchResponse<T>> SearchMultiMatchAsync(Func<FieldsDescriptor<T>, IPromise<Fields>> fields, string searchText, int? skip = null, int? take = null, TextQueryType type = TextQueryType.BestFields, Operator opr = Operator.Or);
        Task<ISearchResponse<T>> GetByIdAsync(TKey id);
        Task DeleteAsync(T document);

    }
}
