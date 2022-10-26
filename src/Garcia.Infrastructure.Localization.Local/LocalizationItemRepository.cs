using Garcia.Application.Contracts.Localization;
using Garcia.Persistence.EntityFramework;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationItemRepository<TContext> : EntityFrameworkRepository<LocalizationItem>, ILocalizationItemRepository<LocalizationItem>
        where TContext : BaseContext
    {
        public LocalizationItemRepository(TContext dbContext) : base(dbContext)
        {
        }
    }
}