using MediatR;

namespace MiniLedger.Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(int Id) : IRequest<bool>;