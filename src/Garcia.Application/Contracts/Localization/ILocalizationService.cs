using System.Threading.Tasks;
using Garcia.Domain;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationService
    {
        bool AddMissingItem { get; }
        Task<string> Localize(string cultureCode, string key);
        Task Localize(string cultureCode, IId<long> item);
    }
}
