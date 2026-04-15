using MediatR;

namespace MiniLedger.Api.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int AccountType { get; set; }
    public bool RequiresParty { get; set; }
}