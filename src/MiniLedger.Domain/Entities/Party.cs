using MiniLedger.Domain.Enums;

namespace MiniLedger.Domain.Entities;

public class Party
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PartyType PartyType { get; set; }
    public string? PhoneNumber { get; set; }

    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}