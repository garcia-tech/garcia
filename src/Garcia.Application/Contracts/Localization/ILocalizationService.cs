using System.Threading.Tasks;
using Garcia.Domain;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationService<T>
        where T : ILocalizationItem
    {
        /// <summary>
        /// The flag for adding missing the <typeparamref name="T"/> item.
        /// </summary>
        bool AddMissingItem { get; }
        /// <summary>
        /// Localizes the <paramref name="key"/> with <paramref name="cultureCode"/>.
        /// If the <typeparamref name="T"/> does not exists by <paramref name="key"/> and <paramref name="cultureCode"/> 
        /// and <see cref="AddMissingItem"/> is <see langword="true"/> adds new <typeparamref name="T"/> item of
        /// <paramref name="key"/> and <paramref name="cultureCode"/>.
        /// </summary>
        /// <param name="cultureCode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> Localize(string cultureCode, string key);
        /// <summary>
        /// Localizes the property of <paramref name="item"/> by <paramref name="cultureCode"/>.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="cultureCode"></param>
        /// <param name="item"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<string> Localize<TItem, TKey>(string cultureCode, TItem item, System.Linq.Expressions.Expression<System.Func<TItem, object>> expression) where TItem : IEntity<TKey>;
        /// <summary>
        /// Localizes the <paramref name="item"/> with <paramref name="cultureCode"/>.
        /// Calls <see cref="ILocalizationService{T}.Localize(string, string)"/> for the inner process.
        /// </summary>
        /// <typeparam name="TKey">The item's type of id.</typeparam>
        /// <param name="cultureCode"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Localize<TKey>(string cultureCode, IEntity<TKey> item);
    }
}
