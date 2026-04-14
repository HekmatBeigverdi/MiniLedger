using MiniLedger.Api.DTOs.Auth;

namespace MiniLedger.Api.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}