using Garcia.Application.Services;
using Garcia.Infrastructure.Localization.Local.Sample.Models;
using Garcia.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Garcia.Infrastructure.Localization.Local.Sample
{
    public class LocalizationSampleDbContext : BaseContext
    {
        public LocalizationSampleDbContext(DbContextOptions options, ILoggedInUserService<long> loggedInUserService) : base(options, loggedInUserService)
        {
        }

        public DbSet<TestModel> TestModels { get; set; }
    }
}
