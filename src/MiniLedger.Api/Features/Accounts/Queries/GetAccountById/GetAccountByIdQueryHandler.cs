using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniLedger.Application.DTOs.Accounts;
using MiniLedger.Infrastructure.Data;

namespace MiniLedger.Api.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly MiniLedgerDbContext _context;

    public GetAccountByIdQueryHandler(MiniLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

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
}