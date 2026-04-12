using System.ComponentModel.DataAnnotations;

namespace MiniLedger.Api.DTOs.JournalEntries;

public class JournalEntryCreateDto
{
    [Required]
    [MaxLength(50)]
    public string EntryNumber { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MinLength(1)]
    public List<JournalEntryLineCreateDto> Lines { get; set; } = new();
}