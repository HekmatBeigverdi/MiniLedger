namespace MiniLedger.Api.DTOs.JournalEntries;

public class JournalEntryDto
{
    public int Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public List<JournalEntryLineDto> Lines { get; set; } = new();
}