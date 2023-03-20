namespace Persistence.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAccount(string name);
    }
}
