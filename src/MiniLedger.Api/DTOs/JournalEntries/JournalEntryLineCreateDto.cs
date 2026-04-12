using System.ComponentModel.DataAnnotations;

namespace MiniLedger.Api.DTOs.JournalEntries;

public class JournalEntryLineCreateDto
{
    [Range(1, int.MaxValue)]
    public int AccountId { get; set; }

    public int? PartyId { get; set; }

    [Range(0, 999999999)]
    public decimal Debit { get; set; }

    [Range(0, 999999999)]
    public decimal Credit { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}