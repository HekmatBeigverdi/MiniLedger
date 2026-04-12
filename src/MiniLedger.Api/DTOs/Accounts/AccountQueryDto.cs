namespace MiniLedger.Api.DTOs.Accounts;

public class AccountQueryDto
{
    public string? Search { get; set; }
    public int? AccountType { get; set; }
    public bool? RequiresParty { get; set; }
    public string? SortBy { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}