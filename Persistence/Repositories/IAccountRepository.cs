namespace Persistence.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAccount(string name);
        Task SetActive(int id, bool active);
        Task<bool> IsActive(int id);
    }
}
