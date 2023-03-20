namespace Persistence.Repositories
{
    public interface IPhoneNumberRepository
    {
        Task CreatePhoneNumber(string phoneNumber, int accountId);
    }
}
