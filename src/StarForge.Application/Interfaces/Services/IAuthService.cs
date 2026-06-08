using StarForge.Application.DTOs.Auth;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pela autenticação de usuários e emissão de tokens JWT.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica o usuário com e-mail e senha e retorna um token JWT válido.
    /// </summary>
    /// <param name="dto">Credenciais de login (e-mail e senha em texto puro).</param>
    /// <returns>Token JWT assinado com os dados do usuário autenticado.</returns>
    /// <exception cref="Exceptions.BusinessRuleException">
    /// Lançada se o e-mail não existir, a senha estiver errada ou a conta estiver desativada.
    /// </exception>
    Task<TokenResponseDto> LoginAsync(LoginDto dto);
}
