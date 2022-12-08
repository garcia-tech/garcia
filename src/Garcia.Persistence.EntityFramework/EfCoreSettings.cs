using Garcia.Infrastructure;

namespace Garcia.Persistence.EntityFramework
{
    public class EfCoreSettings : DatabaseSettings
    {
        public string MigrationsAssembly { get; set; }
    }
}
