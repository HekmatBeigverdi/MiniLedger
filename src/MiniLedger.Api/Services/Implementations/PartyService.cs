using MiniLedger.Api.Common.Enums;
using MiniLedger.Api.DTOs.Parties;
using MiniLedger.Api.Models;
using MiniLedger.Api.Repositories.Interfaces;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Services.Implementations;

public class PartyService : IPartyService
{
    private readonly IPartyRepository _partyRepository;

    public PartyService(IPartyRepository partyRepository)
    {
        _partyRepository = partyRepository;
    }

    public async Task<List<PartyDto>> GetAllAsync()
    {
        var parties = await _partyRepository.GetAllAsync();

        return parties.Select(x => new PartyDto
        {
            Id = x.Id,
            Name = x.Name,
            PartyType = x.PartyType.ToString(),
            PhoneNumber = x.PhoneNumber
        }).ToList();
    }

    public async Task<PartyDto?> GetByIdAsync(int id)
    {
        var party = await _partyRepository.GetByIdAsync(id);
        if (party == null) return null;

        return new PartyDto
        {
            Id = party.Id,
            Name = party.Name,
            PartyType = party.PartyType.ToString(),
            PhoneNumber = party.PhoneNumber
        };
    }

    public async Task<int> CreateAsync(PartyCreateDto dto)
    {
        var party = new Party
        {
            Name = dto.Name,
            PartyType = (PartyType)dto.PartyType,
            PhoneNumber = dto.PhoneNumber
        };

        await _partyRepository.AddAsync(party);
        await _partyRepository.SaveChangesAsync();

        return party.Id;
    }

    public async Task<bool> UpdateAsync(int id, PartyUpdateDto dto)
    {
        var party = await _partyRepository.GetByIdAsync(id);
        if (party == null) return false;

        party.Name = dto.Name;
        party.PartyType = (PartyType)dto.PartyType;
        party.PhoneNumber = dto.PhoneNumber;

        _partyRepository.Update(party);
        await _partyRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var party = await _partyRepository.GetByIdAsync(id);
        if (party == null) return false;

        _partyRepository.Delete(party);
        await _partyRepository.SaveChangesAsync();

        return true;
    }
}