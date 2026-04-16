using MediatR;
using MiniLedger.Application.Interfaces;

namespace MiniLedger.Application.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly IAccountRepository _accountRepository;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        if (account == null) return false;

        _accountRepository.Delete(account);
        await _accountRepository.SaveChangesAsync();

        return true;
    }
}