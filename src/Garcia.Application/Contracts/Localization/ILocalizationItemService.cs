using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationItemService
    {
        List<ILocalizationItem> LocalizationItems { get; }
        Task AddLocalizationItem(ILocalizationItem item);
    }
}
