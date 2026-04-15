using Microsoft.AspNetCore.Identity;

namespace MiniLedger.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}