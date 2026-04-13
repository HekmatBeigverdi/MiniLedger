using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.Accounts;
using MiniLedger.Api.Data;
using MiniLedger.Api.Common.Enums;
using MiniLedger.Api.Models;
using MiniLedger.Api.Repositories.Interfaces;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly MiniLedgerDbContext _context;

    public AccountService(IAccountRepository accountRepository, MiniLedgerDbContext context)
    {
        _accountRepository = accountRepository;
        _context = context;
    }

    public async Task<PagedResponse<List<AccountDto>>> GetAllAsync(AccountQueryDto query)
    {
        var accountsQuery = _context.Accounts
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            accountsQuery = accountsQuery.Where(x =>
                x.Name.Contains(query.Search) ||
                x.Code.Contains(query.Search));
        }

        if (query.AccountType.HasValue)
        {
            accountsQuery = accountsQuery.Where(x => (int)x.AccountType == query.AccountType.Value);
        }

        if (query.RequiresParty.HasValue)
        {
            accountsQuery = accountsQuery.Where(x => x.RequiresParty == query.RequiresParty.Value);
        }

        accountsQuery = query.SortBy?.ToLower() switch
        {
            "name" => accountsQuery.OrderBy(x => x.Name),
            "name_desc" => accountsQuery.OrderByDescending(x => x.Name),
            "code" => accountsQuery.OrderBy(x => x.Code),
            "code_desc" => accountsQuery.OrderByDescending(x => x.Code),
            _ => accountsQuery.OrderBy(x => x.Id)
        };

        var totalCount = await accountsQuery.CountAsync();

        var accounts = await accountsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new AccountDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                AccountType = x.AccountType.ToString(),
                RequiresParty = x.RequiresParty
            })
            .ToListAsync();

        return new PagedResponse<List<AccountDto>>
        {
            Success = true,
            Message = "Accounts retrieved successfully.",
            Data = accounts,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<AccountDto?> GetByIdAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null) return null;

        return new AccountDto
        {
            Id = account.Id,
            Code = account.Code,
            Name = account.Name,
            AccountType = account.AccountType.ToString(),
            RequiresParty = account.RequiresParty
        };
    }

    public async Task<int> CreateAsync(AccountCreateDto dto)
    {
        var codeExists = await _accountRepository.CodeExistsAsync(dto.Code);
        if (codeExists)
            throw new Exception($"Account code '{dto.Code}' already exists.");

        var account = new Account
        {
            Code = dto.Code,
            Name = dto.Name,
            AccountType = (AccountType)dto.AccountType,
            RequiresParty = dto.RequiresParty
        };

        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();

        return account.Id;
    }

    public async Task<bool> UpdateAsync(int id, AccountUpdateDto dto)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null) return false;

        account.Name = dto.Name;
        account.AccountType = (AccountType)dto.AccountType;
        account.RequiresParty = dto.RequiresParty;

        _accountRepository.Update(account);
        await _accountRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null) return false;

        _accountRepository.Delete(account);
        await _accountRepository.SaveChangesAsync();

        return true;
    }
}