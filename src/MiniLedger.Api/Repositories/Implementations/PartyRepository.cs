using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Data;
using MiniLedger.Api.Models;
using MiniLedger.Api.Repositories.Interfaces;

namespace MiniLedger.Api.Repositories.Implementations;

public class PartyRepository : IPartyRepository
{
    private readonly MiniLedgerDbContext _context;

    public PartyRepository(MiniLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Party>> GetAllAsync()
    {
        return await _context.Parties
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Party?> GetByIdAsync(int id)
    {
        return await _context.Parties
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Party party)
    {
        await _context.Parties.AddAsync(party);
    }

    public void Update(Party party)
    {
        _context.Parties.Update(party);
    }

    public void Delete(Party party)
    {
        _context.Parties.Remove(party);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}