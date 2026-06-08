using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StarForge.Application.DTOs.Auth;
using StarForge.Application.DTOs.Usuario;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de autenticação via JWT.
/// Valida as credenciais do usuário e gera um token assinado com as claims necessárias para autorização.
/// </summary>
/// <param name="usuarioRepo">Repositório para buscar o usuário pelo e-mail.</param>
/// <param name="config">Configurações da aplicação — lê <c>Jwt:Key</c>, <c>Jwt:Issuer</c>, etc.</param>
public class AuthService(IUsuarioRepository usuarioRepo, IConfiguration config) : IAuthService
{
    /// <inheritdoc />
    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        // Busca o usuário pelo e-mail; mensagem genérica para não vazar informação
        var usuario = await usuarioRepo.GetByEmailAsync(dto.Email)
            ?? throw new BusinessRuleException("E-mail ou senha inválidos.");

        // Conta desativada não pode autenticar, mesmo com credenciais corretas
        if (!usuario.Ativo)
            throw new BusinessRuleException("Conta desativada.");

        // BCrypt.Verify compara a senha em texto puro com o hash armazenado
        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new BusinessRuleException("E-mail ou senha inválidos.");

        // Usa fallback para evitar NullReferenceException quando a configuração não está presente
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            config["Jwt:Key"] ?? "StarForge@FallbackKey256BitsForDevelopment!!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(int.Parse(config["Jwt:ExpirationHours"] ?? "8"));

        // Claims embutidas no token — acessíveis via User.Claims nos controllers
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()), // ID do usuário para endpoints autenticados
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role)   // Role para [Authorize(Roles = "Admin")]
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        var usuarioDto = new UsuarioDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.Rm,
            usuario.Nivel, usuario.TotalContribuido, usuario.Ativo,
            usuario.Role, usuario.DataCadastro
        );

        return new TokenResponseDto(
            new JwtSecurityTokenHandler().WriteToken(token), // Serializa o token para string
            expiration,
            usuarioDto
        );
    }
}
