using MiniLedger.Api.Models;

namespace MiniLedger.Api.Repositories.Interfaces;

public interface IPartyRepository
{
    Task<List<Party>> GetAllAsync();
    Task<Party?> GetByIdAsync(int id);
    Task AddAsync(Party party);
    void Update(Party party);
    void Delete(Party party);
    Task SaveChangesAsync();
}