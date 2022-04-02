using System.Threading.Tasks;

namespace GarciaCore.Application.Contracts.Marketing
{
    public interface IMarketingService
    {
        Task CreateContactAsync(string email, string name, string surname);
    }
}
