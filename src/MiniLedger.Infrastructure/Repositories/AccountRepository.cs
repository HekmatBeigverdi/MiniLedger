using Microsoft.EntityFrameworkCore;
using MiniLedger.Application.Interfaces;
using MiniLedger.Domain.Entities;
using MiniLedger.Infrastructure.Data;

namespace MiniLedger.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly MiniLedgerDbContext _context;

    public AccountRepository(MiniLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await _context.Accounts
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CodeExistsAsync(string code)
    {
        return await _context.Accounts.AnyAsync(x => x.Code == code);
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public void Update(Account account)
    {
        _context.Accounts.Update(account);
    }

    public void Delete(Account account)
    {
        _context.Accounts.Remove(account);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}