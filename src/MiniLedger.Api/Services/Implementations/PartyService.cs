using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Common.Enums;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.Data;
using MiniLedger.Api.DTOs.Parties;
using MiniLedger.Api.Models;
using MiniLedger.Api.Repositories.Interfaces;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Services.Implementations;

public class PartyService : IPartyService
{
    private readonly IPartyRepository _partyRepository;
    private readonly MiniLedgerDbContext _context;

    public PartyService(IPartyRepository partyRepository, MiniLedgerDbContext context)
    {
        _partyRepository = partyRepository;
        _context = context;
    }

    public async Task<PagedResponse<List<PartyDto>>> GetAllAsync(PartyQueryDto query)
    {
        var partiesQuery = _context.Parties
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            partiesQuery = partiesQuery.Where(x => x.Name.Contains(query.Search));
        }

        if (query.PartyType.HasValue)
        {
            partiesQuery = partiesQuery.Where(x => (int)x.PartyType == query.PartyType.Value);
        }

        partiesQuery = query.SortBy?.ToLower() switch
        {
            "name" => partiesQuery.OrderBy(x => x.Name),
            "name_desc" => partiesQuery.OrderByDescending(x => x.Name),
            _ => partiesQuery.OrderBy(x => x.Id)
        };

        var totalCount = await partiesQuery.CountAsync();

        var parties = await partiesQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new PartyDto
            {
                Id = x.Id,
                Name = x.Name,
                PartyType = x.PartyType.ToString(),
                PhoneNumber = x.PhoneNumber
            })
            .ToListAsync();

        return new PagedResponse<List<PartyDto>>
        {
            Success = true,
            Message = "Parties retrieved successfully.",
            Data = parties,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
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