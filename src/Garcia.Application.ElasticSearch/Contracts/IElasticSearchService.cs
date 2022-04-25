using System.Linq.Expressions;
using Garcia.Application.ElasticSearch.Models;
using Nest;

namespace Garcia.Application.ElasticSearch.Contracts
{
    public interface IElasticSearchService<T, TKey>
        where T : ElasticSearchDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Saves the document, if document is exists, overwrites exists document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns>Returns the index of the saved document.</returns>
        Task<long> SetDocumentAsync(T document);
        /// <summary>
        /// The <paramref name="searchText"/> is searched over the target field.
        /// </summary>
        /// <param name="field">Target field</param>
        /// <param name="searchText">Desired text</param>
        /// <param name="skip">Offset</param>
        /// <param name="take">Count</param>
        /// <returns>Returns <see cref="ISearchResponse{TDocument}"/>. <see cref="ISearchResponse{TDocument}.Documents"/> gives <see cref="IReadOnlyCollection{T}"/> of documents</returns>
        Task<ISearchResponse<T>> SearchAsync(Expression<Func<T, object>> field, string searchText, int? skip = null, int? take = null);
        Task<ISearchResponse<T>> SearchAsync(SearchRequest<T> request);
        Task<ISearchResponse<T>> SearchAsync(SearchDescriptor<T> query);
        /// <summary>
        /// The <paramref name="searchText"/> is searched over the target fields.
        /// </summary>
        /// <param name="fields">Target fields</param>
        /// <param name="searchText"></param>
        /// <param name="skip">Offset</param>
        /// <param name="take">Count</param>
        /// <param name="type"></param>
        /// <param name="opr">And&Or operator for query.</param>
        /// <returns>Returns <see cref="ISearchResponse{TDocument}"/>. <see cref="ISearchResponse{TDocument}.Documents"/> gives <see cref="IReadOnlyCollection{T}"/> of documents</returns>
        Task<ISearchResponse<T>> SearchMultiMatchAsync(Func<FieldsDescriptor<T>, IPromise<Fields>> fields, string searchText, int? skip = null, int? take = null, TextQueryType type = TextQueryType.BestFields, Operator opr = Operator.Or);
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns single document that matches the id.</returns>
        Task<T> GetByIdAsync(TKey id);
        /// <summary>
        /// Deletes given document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task DeleteAsync(T document);

    }
}
