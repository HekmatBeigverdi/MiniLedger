namespace MiniLedger.Api.DTOs.JournalEntries;

public class JournalEntryLineDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public int? PartyId { get; set; }
    public string? PartyName { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}