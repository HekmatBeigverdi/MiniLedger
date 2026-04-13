using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.Data;
using MiniLedger.Api.DTOs.JournalEntries;
using MiniLedger.Api.Models;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Services.Implementations;

public class JournalEntryService : IJournalEntryService
{
    private readonly MiniLedgerDbContext _context;
    private readonly ILogger<JournalEntryService> _logger;

    public JournalEntryService(MiniLedgerDbContext context, ILogger<JournalEntryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CreateAsync(JournalEntryCreateDto dto)
    {
        if (dto.Lines == null || dto.Lines.Count == 0)
            throw new Exception("Journal entry must have at least one line.");

        var entryNumberExists = await _context.JournalEntries
            .AnyAsync(x => x.EntryNumber == dto.EntryNumber);

        if (entryNumberExists)
            throw new Exception($"Entry number '{dto.EntryNumber}' already exists.");

        var totalDebit = dto.Lines.Sum(x => x.Debit);
        var totalCredit = dto.Lines.Sum(x => x.Credit);

        if (totalDebit != totalCredit)
            throw new Exception("Total debit and total credit must be equal.");

        foreach (var line in dto.Lines)
        {
            if ((line.Debit > 0 && line.Credit > 0) || (line.Debit == 0 && line.Credit == 0))
                throw new Exception("Each line must have either debit or credit.");

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == line.AccountId);
            if (account == null)
                throw new Exception($"Account with id {line.AccountId} not found.");

            if (account.RequiresParty && !line.PartyId.HasValue)
                throw new Exception($"Account '{account.Name}' requires a party.");

            if (line.PartyId.HasValue)
            {
                var partyExists = await _context.Parties.AnyAsync(x => x.Id == line.PartyId.Value);
                if (!partyExists)
                    throw new Exception($"Party with id {line.PartyId.Value} not found.");
            }
        }

        var entry = new JournalEntry
        {
            EntryNumber = dto.EntryNumber,
            Date = dto.Date,
            Description = dto.Description,
            Lines = dto.Lines.Select(x => new JournalEntryLine
            {
                AccountId = x.AccountId,
                PartyId = x.PartyId,
                Debit = x.Debit,
                Credit = x.Credit,
                Description = x.Description
            }).ToList()
        };

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Journal entry {EntryNumber} created with id {Id}", entry.EntryNumber, entry.Id);

        return entry.Id;
    }

    public async Task<JournalEntryDto?> GetByIdAsync(int id)
    {
        var entry = await _context.JournalEntries
            .AsNoTracking()
            .Include(x => x.Lines)
                .ThenInclude(x => x.Account)
            .Include(x => x.Lines)
                .ThenInclude(x => x.Party)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entry == null) return null;

        return new JournalEntryDto
        {
            Id = entry.Id,
            EntryNumber = entry.EntryNumber,
            Date = entry.Date,
            Description = entry.Description,
            Lines = entry.Lines.Select(x => new JournalEntryLineDto
            {
                Id = x.Id,
                AccountId = x.AccountId,
                AccountName = x.Account.Name,
                PartyId = x.PartyId,
                PartyName = x.Party != null ? x.Party.Name : null,
                Debit = x.Debit,
                Credit = x.Credit,
                Description = x.Description
            }).ToList()
        };
    }

    public async Task<PagedResponse<List<JournalEntryListItemDto>>> GetAllAsync(JournalEntryQueryDto query)
    {
        var entriesQuery = _context.JournalEntries
            .AsNoTracking()
            .Include(x => x.Lines)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.EntryNumber))
        {
            entriesQuery = entriesQuery.Where(x => x.EntryNumber.Contains(query.EntryNumber));
        }

        if (query.FromDate.HasValue)
        {
            entriesQuery = entriesQuery.Where(x => x.Date >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            entriesQuery = entriesQuery.Where(x => x.Date <= query.ToDate.Value);
        }

        entriesQuery = query.SortBy?.ToLower() switch
        {
            "date" => entriesQuery.OrderBy(x => x.Date),
            "date_desc" => entriesQuery.OrderByDescending(x => x.Date),
            "entrynumber" => entriesQuery.OrderBy(x => x.EntryNumber),
            "entrynumber_desc" => entriesQuery.OrderByDescending(x => x.EntryNumber),
            _ => entriesQuery.OrderByDescending(x => x.Date)
        };

        var totalCount = await entriesQuery.CountAsync();

        var items = await entriesQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new JournalEntryListItemDto
            {
                Id = x.Id,
                EntryNumber = x.EntryNumber,
                Date = x.Date,
                Description = x.Description,
                TotalDebit = x.Lines.Sum(l => l.Debit),
                TotalCredit = x.Lines.Sum(l => l.Credit),
                LinesCount = x.Lines.Count
            })
            .ToListAsync();

        return new PagedResponse<List<JournalEntryListItemDto>>
        {
            Success = true,
            Message = "Journal entries retrieved successfully.",
            Data = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}