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

public class AuthService(IUsuarioRepository usuarioRepo, IConfiguration config) : IAuthService
{
    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await usuarioRepo.GetByEmailAsync(dto.Email)
            ?? throw new BusinessRuleException("E-mail ou senha inválidos.");

        if (!usuario.Ativo)
            throw new BusinessRuleException("Conta desativada.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new BusinessRuleException("E-mail ou senha inválidos.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(int.Parse(config["Jwt:ExpirationHours"] ?? "8"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role)
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
            new JwtSecurityTokenHandler().WriteToken(token),
            expiration,
            usuarioDto
        );
    }
}
