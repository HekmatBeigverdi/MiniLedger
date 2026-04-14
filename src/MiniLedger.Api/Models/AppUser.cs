using Microsoft.AspNetCore.Identity;

namespace MiniLedger.Api.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}