using System.ComponentModel.DataAnnotations;

namespace MiniLedger.Api.DTOs.Accounts;

public class AccountUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 5)]
    public int AccountType { get; set; }

    public bool RequiresParty { get; set; }
}