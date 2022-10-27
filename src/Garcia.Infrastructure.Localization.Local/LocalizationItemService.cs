using Garcia.Application.Contracts.Localization;
using Garcia.Application.Contracts.Persistence;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationItemService<T, TKey> : ILocalizationItemService<T>
        where T : LocalizationItem<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private readonly IAsyncRepository<T, TKey> _localizationItemRepository;

        public LocalizationItemService(IAsyncRepository<T, TKey> localizationItemRepository)
        {
            _localizationItemRepository = localizationItemRepository;
        }

        public async Task AddLocalizationItem(T item)
        {
            await _localizationItemRepository.AddAsync(item);
        }

        public async Task<T> GetLocalizationItem(string key, string cultureCode)
        {
            return (T)(await _localizationItemRepository.GetAsync(x => x.Key == key && x.CultureCode == cultureCode))
                .FirstOrDefault();
        }
    }
}