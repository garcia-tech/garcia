using System.Linq.Expressions;
using GarciaCore.Application.ElasticSearch.Contracts;
using GarciaCore.Application.ElasticSearch.Models;
using GarciaCore.Exceptions.ElasticSearch;
using Nest;

namespace GarciaCore.Infrastructure.ElasticSearch
{
    public class ElasticSearchService<T, TKey> : IElasticSearchService<T, TKey>
        where T : ElasticSearchDocument<TKey> 
        where TKey : IEquatable<TKey>

    {
        private readonly IElasticClient _client;

        public ElasticSearchService(ElasticSearchConnectionFactory factory)
        {
            _client = factory.GetClient<T>();
        }

        public async Task<long> SetDocumentAsync(T document)
        {
            var index = typeof(T).Name.ToLower();
            var exists = await _client.DocumentExistsAsync<T>(DocumentPath<T>.Id(document), x => x.Index(index));

            if (exists.Exists)
            {
                var updateResult = await _client.UpdateAsync(DocumentPath<T>.Id(document), x => x.Index(index).Doc(document).RetryOnConflict(3));
                if (updateResult.ServerError == null) return updateResult.SequenceNumber;
                throw new SetDocumentException($"Update Document failed at index {index} :" + updateResult.ServerError.Error.Reason);
            }

            var insertResult = await _client.IndexDocumentAsync(document);
            if (insertResult.ServerError == null) return insertResult.SequenceNumber;
            throw new SetDocumentException($"Insert Document failed at index {index} :" + insertResult.ServerError.Error.Reason);
        }

        public async Task<ISearchResponse<T>> SearchAsync(Expression<Func<T, object>> field , string searchText, int? skip = null, int? take = null)
        {
            var query = new SearchDescriptor<T>()
                .Query(q => q
                    .Match(m => m
                    .Field(field)
                    .Query(searchText)
                    )
                );

            if(skip != null && take != null)
            {
                query.Skip(skip).Take(take);
            }

            var result = await _client.SearchAsync<T>(query);
            return result;
        }

        public async Task<ISearchResponse<T>> SearchAsync(SearchRequest<T> request)
        {
            var result = await _client.SearchAsync<T>(request);
            return result;
        }

        public async Task<ISearchResponse<T>> SearchAsync(SearchDescriptor<T> query)
        {
            var result = await _client.SearchAsync<T>(query);
            return result;
        }

        public async Task<ISearchResponse<T>> SearchMultiMatchAsync(Func<FieldsDescriptor<T>, IPromise<Fields>> fields, string searchText, int? skip = null, int? take = null, TextQueryType type = TextQueryType.BestFields, Operator opr = Operator.Or)
        {
            var query = new SearchDescriptor<T>()
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(fields)
                         .Query(searchText)
                         .Type(type)
                         .Operator(opr)
                         )
                    );

            if (skip != null && take != null)
            {
                query.Skip(skip).Take(take);
            }

            var result = await _client.SearchAsync<T>(query);
            return result;
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            var result = await _client.SearchAsync<T>(s => s
                .Query(q =>
                    q.Term(t => t.Id, id))
            );
            return result.Documents.FirstOrDefault();
        }

        public async Task DeleteAsync(T document)
        {
            var index = typeof(T).Name.ToLower();
            var result = await _client.DeleteAsync<T>(document);
            if (result.ServerError != null) throw new DeleteDocumentException($"Delete Document failed at index {index} :" + result.ServerError.Error.Reason);
        }
    }
}
