using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniLedger.Domain.Enums;
using MiniLedger.Infrastructure.Data;

namespace MiniLedger.Api.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, bool>
{
    private readonly MiniLedgerDbContext _context;

    public UpdateAccountCommandHandler(MiniLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (account == null) return false;

        account.Name = request.Name;
        account.AccountType = (AccountType)request.AccountType;
        account.RequiresParty = request.RequiresParty;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}