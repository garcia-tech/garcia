using Garcia.Application.Contracts.Localization;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationItemService : ILocalizationItemService
    {
        public List<ILocalizationItem> LocalizationItems { get; } = new List<ILocalizationItem>();

        public async Task AddLocalizationItem(ILocalizationItem item)
        {
            LocalizationItems.Add(item);
        }
    }
}