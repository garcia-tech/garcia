using Garcia.Application.Contracts.Localization;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationItemService : ILocalizationItemService
    {
        private readonly ILocalizationItemRepository<LocalizationItem> _localizationItemRepository;

        public LocalizationItemService(ILocalizationItemRepository<LocalizationItem> localizationItemRepository)
        {
            _localizationItemRepository = localizationItemRepository;
        }

        public async Task AddLocalizationItem(ILocalizationItem item)
        {
            await _localizationItemRepository.AddAsync(item as LocalizationItem);
        }

        public async Task<ILocalizationItem> GetLocalizationItem(string key, string cultureCode)
        {
            return (await _localizationItemRepository.GetAsync(x => x.Key == key && x.CultureCode == cultureCode))
                .FirstOrDefault();
        }
    }
}