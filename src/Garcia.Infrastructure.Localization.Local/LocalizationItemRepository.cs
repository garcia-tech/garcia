using Garcia.Application.Contracts.Localization;
using Garcia.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationItemRepository : EntityFrameworkRepository<LocalizationItem>, ILocalizationItemRepository<LocalizationItem>
    {
        public LocalizationItemRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}