using MediatR;

namespace MiniLedger.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AccountType { get; set; }
    public bool RequiresParty { get; set; }
}