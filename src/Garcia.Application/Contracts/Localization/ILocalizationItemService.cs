using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationItemService<T>
        where T : ILocalizationItem
    {
        /// <summary>
        /// Adds the <typeparamref name="T"/> item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task AddLocalizationItem(T item);
        /// <summary>
        /// Gets the <typeparamref name="T"/> item <paramref name="key"/> key and <paramref name="cultureCode"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        Task<T> GetLocalizationItem(string key, string cultureCode);
    }
}
