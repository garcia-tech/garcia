using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationItemService
    {
        Task AddLocalizationItem(ILocalizationItem item);
        Task<ILocalizationItem> GetLocalizationItem(string key, string cultureCode);
    }
}
