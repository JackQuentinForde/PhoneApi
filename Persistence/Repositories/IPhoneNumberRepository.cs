using Domain.Entities;

namespace Persistence.Repositories
{
    public interface IPhoneNumberRepository
    {
        Task CreatePhoneNumber(string phoneNumber, int accountId);
        Task DeletePhoneNumber(int Id);
        Task<PhoneNumber?> GetPhoneNumber(int id);
        Task<IReadOnlyCollection<PhoneNumber>> GetAll(int accountId);
    }
}
