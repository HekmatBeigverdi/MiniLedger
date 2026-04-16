using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniLedger.Infrastructure.Data;

namespace MiniLedger.Api.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly MiniLedgerDbContext _context;

    public DeleteAccountCommandHandler(MiniLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (account == null) return false;

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}