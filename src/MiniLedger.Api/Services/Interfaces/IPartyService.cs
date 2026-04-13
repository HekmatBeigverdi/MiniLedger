using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.DTOs.Parties;

namespace MiniLedger.Api.Services.Interfaces;

public interface IPartyService
{
    Task<PagedResponse<List<PartyDto>>> GetAllAsync(PartyQueryDto query);
    Task<PartyDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(PartyCreateDto dto);
    Task<bool> UpdateAsync(int id, PartyUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}