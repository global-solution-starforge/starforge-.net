using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using StarForge.Application.DTOs.Auth;
using StarForge.Application.DTOs.Usuario;
using StarForge.Application.Interfaces;
using StarForge.Domain.Entities;

namespace StarForge.IntegrationTests;

public class AuthControllerTests(StarForgeWebApplicationFactory factory)
    : IClassFixture<StarForgeWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task SeedUsuarioAsync(string email, string senha)
    {
        using var scope = factory.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
        var hash = BCrypt.Net.BCrypt.HashPassword(senha);
        var usuario = new Usuario("Teste", email, hash, "User");
        await repo.AddAsync(usuario);
        await repo.SaveChangesAsync();
    }

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
    {
        const string email = "auth_test@fiap.com.br";
        const string senha = "Senha@123";
        await SeedUsuarioAsync(email, senha);

        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto(email, senha));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        Assert.NotNull(result?.Token);
        Assert.NotEmpty(result!.Token);
    }

    [Fact]
    public async Task Login_ComSenhaErrada_DeveRetornar422()
    {
        const string email = "auth_wrong@fiap.com.br";
        await SeedUsuarioAsync(email, "SenhaCorreta@123");

        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto(email, "SenhaErrada@999"));

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task Login_EmailNaoExistente_DeveRetornar422()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginDto("naoexiste@fiap.com.br", "qualquersenha"));

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }
}
