using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.JournalEntries;

namespace MiniLedger.Api.Services.Interfaces;

public interface IJournalEntryService
{
    Task<int> CreateAsync(JournalEntryCreateDto dto);
    Task<JournalEntryDto?> GetByIdAsync(int id);
    Task<PagedResponse<List<JournalEntryListItemDto>>> GetAllAsync(JournalEntryQueryDto query);
}