using MediatR;
using MiniLedger.Application.Interfaces;
using MiniLedger.Domain.Enums;

namespace MiniLedger.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, bool>
{
    private readonly IAccountRepository _accountRepository;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        if (account == null) return false;

        account.Name = request.Name;
        account.AccountType = (AccountType)request.AccountType;
        account.RequiresParty = request.RequiresParty;

        _accountRepository.Update(account);
        await _accountRepository.SaveChangesAsync();

        return true;
    }
}