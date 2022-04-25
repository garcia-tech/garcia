using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Marketing
{
    public interface IMarketingService
    {
        Task CreateContactAsync(string email, string name, string surname);
    }
}
