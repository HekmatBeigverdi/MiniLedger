using MiniLedger.Domain.Enums;

namespace MiniLedger.Domain.Entities;

public class Account
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public bool RequiresParty { get; set; }

    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}