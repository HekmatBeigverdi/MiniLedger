using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.Features.Accounts.Commands.CreateAccount;
using MiniLedger.Api.Features.Accounts.Commands.DeleteAccount;
using MiniLedger.Api.Features.Accounts.Commands.UpdateAccount;
using MiniLedger.Api.Features.Accounts.Queries.GetAccountById;
using MiniLedger.Api.Features.Accounts.Queries.GetAccounts;
using MiniLedger.Application.DTOs.Accounts;

namespace MiniLedger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<List<AccountDto>>>> GetAll([FromQuery] GetAccountsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(AccountCreateDto dto)
    {
        var command = new CreateAccountCommand
        {
            Code = dto.Code,
            Name = dto.Name,
            AccountType = dto.AccountType,
            RequiresParty = dto.RequiresParty
        };

        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, AccountUpdateDto dto)
    {
        var command = new UpdateAccountCommand
        {
            Id = id,
            Name = dto.Name,
            AccountType = dto.AccountType,
            RequiresParty = dto.RequiresParty
        };

        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteAccountCommand(id));
        if (!deleted) return NotFound();

        return NoContent();
    }
}