using StarForge.Domain.Entities;

namespace StarForge.UnitTests.Domain;

public class UsuarioTests
{
    private static Usuario CriarUsuario() =>
        new("Tiago Costa", "tiago@fiap.com.br", "RM12345", "hash");

    [Fact]
    public void AdicionarContribuicao_MenorQue100_DeveManterRecrutaNivel()
    {
        var usuario = CriarUsuario();
        usuario.AdicionarContribuicao(50m);
        Assert.Equal("RECRUTA", usuario.Nivel);
        Assert.Equal(50m, usuario.TotalContribuido);
    }

    [Fact]
    public void AdicionarContribuicao_Entre100E499_DeveSetarOperativoNivel()
    {
        var usuario = CriarUsuario();
        usuario.AdicionarContribuicao(100m);
        Assert.Equal("OPERATIVO", usuario.Nivel);
    }

    [Fact]
    public void AdicionarContribuicao_Entre500E1999_DeveSetarVeteranoNivel()
    {
        var usuario = CriarUsuario();
        usuario.AdicionarContribuicao(500m);
        Assert.Equal("VETERANO", usuario.Nivel);
    }

    [Fact]
    public void AdicionarContribuicao_AcimaDe2000_DeveSetarComandanteNivel()
    {
        var usuario = CriarUsuario();
        usuario.AdicionarContribuicao(2000m);
        Assert.Equal("COMANDANTE", usuario.Nivel);
    }

    [Fact]
    public void AdicionarContribuicao_AcumulaCorretamente()
    {
        var usuario = CriarUsuario();
        usuario.AdicionarContribuicao(300m);
        usuario.AdicionarContribuicao(300m);
        Assert.Equal(600m, usuario.TotalContribuido);
        Assert.Equal("VETERANO", usuario.Nivel);
    }

    [Fact]
    public void Desativar_DeveSetarAtivoFalse()
    {
        var usuario = CriarUsuario();
        usuario.Desativar();
        Assert.False(usuario.Ativo);
    }
}
