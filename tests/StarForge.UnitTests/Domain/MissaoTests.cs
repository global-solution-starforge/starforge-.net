using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.UnitTests.Domain;

public class MissaoTests
{
    private static Missao CriarMissao(decimal meta = 1000m, DateTime? dataLimite = null) =>
        new("Missão Alfa", "Descrição", meta,
            DateTime.UtcNow.AddDays(-1),
            dataLimite ?? DateTime.UtcNow.AddDays(30));

    [Fact]
    public void VerificarMeta_QuandoArrecadadoMenorQueMeta_DeveManterAtiva()
    {
        var missao = CriarMissao(meta: 1000m);
        missao.RegistrarContribuicao(500m);
        missao.VerificarMeta();
        Assert.Equal(StatusMissao.Ativa, missao.Status);
    }

    [Fact]
    public void VerificarMeta_QuandoArrecadadoIgualMeta_DeveDefinirComoConcluida()
    {
        var missao = CriarMissao(meta: 1000m);
        missao.RegistrarContribuicao(1000m);
        missao.VerificarMeta();
        Assert.Equal(StatusMissao.Concluida, missao.Status);
    }

    [Fact]
    public void VerificarMeta_QuandoArrecadadoMaiorQueMeta_DeveDefinirComoConcluida()
    {
        var missao = CriarMissao(meta: 1000m);
        missao.RegistrarContribuicao(1500m);
        missao.VerificarMeta();
        Assert.Equal(StatusMissao.Concluida, missao.Status);
    }

    [Fact]
    public void VerificarMeta_QuandoPrazoExpiradoSemMeta_DeveDefinirComoFalhou()
    {
        var missao = new Missao("Missão Beta", "Desc", 1000m,
            DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-1));
        missao.RegistrarContribuicao(500m);
        missao.VerificarMeta();
        Assert.Equal(StatusMissao.Falhou, missao.Status);
    }

    [Fact]
    public void RegistrarContribuicao_DeveAcumularTotalArrecadado()
    {
        var missao = CriarMissao();
        missao.RegistrarContribuicao(200m);
        missao.RegistrarContribuicao(300m);
        Assert.Equal(500m, missao.TotalArrecadado);
    }
}
