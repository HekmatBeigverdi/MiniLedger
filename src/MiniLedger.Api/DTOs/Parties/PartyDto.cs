namespace MiniLedger.Api.DTOs.Parties;

public class PartyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PartyType { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}