using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace GarciaCore.Persistence.EntityFramework
{
    public static class EntityFrameworkRegistration
    {
        public static IServiceCollection AddEfCore<TContext>(this IServiceCollection service, Action<DbContextOptionsBuilder> options) where TContext : BaseContext
        {
            return service.AddDbContext<TContext>(options);
        }
    }
}
