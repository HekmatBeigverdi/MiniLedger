using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.Accounts;

namespace MiniLedger.Api.Services.Interfaces;

public interface IAccountService
{
    Task<PagedResponse<List<AccountDto>>> GetAllAsync(AccountQueryDto query);
    Task<AccountDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(AccountCreateDto dto);
    Task<bool> UpdateAsync(int id, AccountUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}