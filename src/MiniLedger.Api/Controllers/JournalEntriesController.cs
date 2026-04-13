using Microsoft.AspNetCore.Mvc;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.JournalEntries;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JournalEntriesController : ControllerBase
{
    private readonly IJournalEntryService _journalEntryService;

    public JournalEntriesController(IJournalEntryService journalEntryService)
    {
        _journalEntryService = journalEntryService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<List<JournalEntryListItemDto>>>> GetAll([FromQuery] JournalEntryQueryDto query)
    {
        var result = await _journalEntryService.GetAllAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JournalEntryDto>> GetById(int id)
    {
        var result = await _journalEntryService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create(JournalEntryCreateDto dto)
    {
        var id = await _journalEntryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}