using System.ComponentModel.DataAnnotations;

namespace MiniLedger.Api.DTOs.Parties;

public class PartyUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 3)]
    public int PartyType { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
}