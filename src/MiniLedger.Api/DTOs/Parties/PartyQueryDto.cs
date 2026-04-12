namespace MiniLedger.Api.DTOs.Parties;

public class PartyQueryDto
{
    public string? Search { get; set; }
    public int? PartyType { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}