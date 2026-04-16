using MediatR;

namespace MiniLedger.Api.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(int Id) : IRequest<bool>;