using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Infrastructure;

namespace Garcia.Persistence.EntityFramework
{
    public class EfCoreSettings : DatabaseSettings
    {
        public string MigrationsAssembly { get; set; }
    }
}
