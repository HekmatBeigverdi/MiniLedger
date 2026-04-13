using Microsoft.AspNetCore.Mvc;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.Accounts;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<List<AccountDto>>>> GetAll([FromQuery] AccountQueryDto query)
    {
        var result = await _accountService.GetAllAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountDto>> GetById(int id)
    {
        var result = await _accountService.GetByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(AccountCreateDto dto)
    {
        var id = await _accountService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, AccountUpdateDto dto)
    {
        var updated = await _accountService.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _accountService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}