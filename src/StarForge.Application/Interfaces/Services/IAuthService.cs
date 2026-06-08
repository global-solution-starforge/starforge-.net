using StarForge.Application.DTOs.Auth;

namespace StarForge.Application.Interfaces.Services;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginDto dto);
}
