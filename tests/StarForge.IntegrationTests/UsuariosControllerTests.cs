using System.Net;
using System.Net.Http.Json;
using StarForge.Application.DTOs.Usuario;

namespace StarForge.IntegrationTests;

public class UsuariosControllerTests(StarForgeWebApplicationFactory factory)
    : IClassFixture<StarForgeWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostUsuario_DadosValidos_DeveRetornar201()
    {
        var dto = new CriarUsuarioDto(
            "João Piloto",
            $"joao_{Guid.NewGuid():N}@fiap.com.br",
            "Senha@123456"
        );

        var response = await _client.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<UsuarioDto>();
        Assert.NotNull(result);
        Assert.Equal(dto.Nome, result!.Nome);
        Assert.Equal("RECRUTA", result.Nivel);
    }

    [Fact]
    public async Task PostUsuario_EmailDuplicado_DeveRetornar422()
    {
        var email = $"dup_{Guid.NewGuid():N}@fiap.com.br";

        await _client.PostAsJsonAsync("/api/usuarios",
            new CriarUsuarioDto("Piloto 1", email, "Senha@123456"));

        var response = await _client.PostAsJsonAsync("/api/usuarios",
            new CriarUsuarioDto("Piloto 2", email, "Senha@123456"));

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }
}
