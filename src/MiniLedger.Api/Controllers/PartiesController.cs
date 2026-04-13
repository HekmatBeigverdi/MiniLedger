using Microsoft.AspNetCore.Mvc;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.Parties;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartiesController : ControllerBase
{
    private readonly IPartyService _partyService;

    public PartiesController(IPartyService partyService)
    {
        _partyService = partyService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<List<PartyDto>>>> GetAll([FromQuery] PartyQueryDto query)
    {
        var result = await _partyService.GetAllAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PartyDto>> GetById(int id)
    {
        var result = await _partyService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(PartyCreateDto dto)
    {
        var id = await _partyService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, PartyUpdateDto dto)
    {
        var updated = await _partyService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _partyService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}