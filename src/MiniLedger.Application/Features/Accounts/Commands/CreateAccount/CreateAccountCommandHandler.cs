using MediatR;
using MiniLedger.Application.Interfaces;
using MiniLedger.Domain.Entities;
using MiniLedger.Domain.Enums;

namespace MiniLedger.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var codeExists = await _accountRepository.CodeExistsAsync(request.Code);
        if (codeExists)
            throw new Exception($"Account code '{request.Code}' already exists.");

        var account = new Account
        {
            Code = request.Code,
            Name = request.Name,
            AccountType = (AccountType)request.AccountType,
            RequiresParty = request.RequiresParty
        };

        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();

        return account.Id;
    }
}