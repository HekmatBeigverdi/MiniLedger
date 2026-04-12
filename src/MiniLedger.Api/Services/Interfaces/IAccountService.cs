using MiniLedger.Api.DTOs.Accounts;

namespace MiniLedger.Api.Services.Interfaces;

public interface IAccountService
{
    Task<List<AccountDto>> GetAllAsync();
    Task<AccountDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(AccountCreateDto dto);
    Task<bool> UpdateAsync(int id, AccountUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}