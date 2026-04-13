namespace MiniLedger.Api.DTOs.JournalEntries;

public class JournalEntryListItemDto
{
    public int Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public int LinesCount { get; set; }
}