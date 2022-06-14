using Garcia.Application.Contracts.Persistence;
using Garcia.Domain;

namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationItemRepository<T> : IAsyncRepository<T>
        where T : Entity<long>, ILocalizationItem
    {
    }
}
