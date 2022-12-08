using System.Threading.Tasks;

namespace Garcia.Application.Contracts.Marketing
{
    public interface IMarketingService
    {
        /// <summary>
        /// Creates new contact.
        /// </summary>
        /// <param name="email">The contact's email.</param>
        /// <param name="name">The contact's name.</param>
        /// <param name="surname">The contact's surname.</param>
        /// <returns></returns>
        Task CreateContactAsync(string email, string name, string surname);
    }
}
