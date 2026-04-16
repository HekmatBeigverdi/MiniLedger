using MediatR;
using MiniLedger.Application.DTOs.Accounts;

namespace MiniLedger.Application.Features.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(int Id) : IRequest<AccountDto?>;