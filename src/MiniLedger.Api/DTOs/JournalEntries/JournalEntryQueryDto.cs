namespace MiniLedger.Api.DTOs.JournalEntries;

public class JournalEntryQueryDto
{
    public string? EntryNumber { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}