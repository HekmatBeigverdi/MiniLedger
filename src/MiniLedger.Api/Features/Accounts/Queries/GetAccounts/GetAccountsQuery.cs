using MediatR;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Application.DTOs.Accounts;

namespace MiniLedger.Api.Features.Accounts.Queries.GetAccounts;

public class GetAccountsQuery : IRequest<PagedResponse<List<AccountDto>>>
{
    public string? Search { get; set; }
    public int? AccountType { get; set; }
    public bool? RequiresParty { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}