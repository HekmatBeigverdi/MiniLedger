using MiniLedger.Api.Common.Enums;
using MiniLedger.Api.DTOs.Accounts;
using MiniLedger.Api.Models;
using MiniLedger.Api.Repositories.Interfaces;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<List<AccountDto>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();

        return accounts.Select(x => new AccountDto
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            AccountType = x.AccountType.ToString(),
            RequiresParty = x.RequiresParty
        }).ToList();
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