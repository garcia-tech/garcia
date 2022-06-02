using Garcia.Domain;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationService
    {
        bool AddMissingItem { get; }
        Task<string> Localize(string cultureCode, string key);
        Task Localize(string cultureCode, IId<long> item);
    }
}
