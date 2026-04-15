using MiniLedger.Domain.Entities;

namespace MiniLedger.Application.Interfaces;

public interface IAccountRepository
{
    Task<List<Account>> GetAllAsync();
    Task<Account?> GetByIdAsync(int id);
    Task<bool> CodeExistsAsync(string code);
    Task AddAsync(Account account);
    void Update(Account account);
    void Delete(Account account);
    Task SaveChangesAsync();
}