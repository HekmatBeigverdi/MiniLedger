using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Application.DTOs.Accounts;
using MiniLedger.Infrastructure.Data;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Features.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, PagedResponse<List<AccountDto>>>
{
    private readonly MiniLedgerDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public GetAccountsQueryHandler(
        MiniLedgerDbContext context,
        ICacheService cacheService,
        IConfiguration configuration)
    {
        _context = context;
        _cacheService = cacheService;
        _configuration = configuration;
    }

    public async Task<PagedResponse<List<AccountDto>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"accounts:{request.Search}:{request.AccountType}:{request.RequiresParty}:{request.SortBy}:{request.PageNumber}:{request.PageSize}";
        var durationSeconds = _configuration.GetValue<int>("Caching:AccountsDurationSeconds", 60);

        return (await _cacheService.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                var query = _context.Accounts
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    query = query.Where(x =>
                        x.Name.Contains(request.Search) ||
                        x.Code.Contains(request.Search));
                }

                if (request.AccountType.HasValue)
                {
                    query = query.Where(x => (int)x.AccountType == request.AccountType.Value);
                }

                if (request.RequiresParty.HasValue)
                {
                    query = query.Where(x => x.RequiresParty == request.RequiresParty.Value);
                }

                query = request.SortBy?.ToLower() switch
                {
                    "name" => query.OrderBy(x => x.Name),
                    "name_desc" => query.OrderByDescending(x => x.Name),
                    "code" => query.OrderBy(x => x.Code),
                    "code_desc" => query.OrderByDescending(x => x.Code),
                    _ => query.OrderBy(x => x.Id)
                };

                var totalCount = await query.CountAsync(cancellationToken);

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new AccountDto
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        AccountType = x.AccountType.ToString(),
                        RequiresParty = x.RequiresParty
                    })
                    .ToListAsync(cancellationToken);

                return new PagedResponse<List<AccountDto>>
                {
                    Success = true,
                    Message = "Accounts retrieved successfully.",
                    Data = items,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };
            },
            TimeSpan.FromSeconds(durationSeconds)))!;
    }
            
}