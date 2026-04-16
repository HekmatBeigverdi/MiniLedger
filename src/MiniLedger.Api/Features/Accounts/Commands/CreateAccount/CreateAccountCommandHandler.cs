using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniLedger.Domain.Entities;
using MiniLedger.Domain.Enums;
using MiniLedger.Infrastructure.Data;

namespace MiniLedger.Api.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly MiniLedgerDbContext _context;

    public CreateAccountCommandHandler(MiniLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var codeExists = await _context.Accounts.AnyAsync(x => x.Code == request.Code, cancellationToken);
        if (codeExists)
            throw new Exception($"Account code '{request.Code}' already exists.");

        var account = new Account
        {
            Code = request.Code,
            Name = request.Name,
            AccountType = (AccountType)request.AccountType,
            RequiresParty = request.RequiresParty
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}