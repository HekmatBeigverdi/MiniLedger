using MediatR;
using MiniLedger.Application.DTOs.Accounts;
using MiniLedger.Application.Interfaces;

namespace MiniLedger.Application.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
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