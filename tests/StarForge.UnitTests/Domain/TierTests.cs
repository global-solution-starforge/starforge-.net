using StarForge.Domain.Entities;
using StarForge.Domain.Exceptions;

namespace StarForge.UnitTests.Domain;

public class TierTests
{
    private static Tier CriarTier(int limiteVagas = 2) =>
        new("Bronze", 100m, "Uma nave de bronze", limiteVagas, Guid.NewGuid());

    [Fact]
    public void OcuparVaga_DeveIncrementarVagasOcupadas()
    {
        var tier = CriarTier(limiteVagas: 3);
        tier.OcuparVaga();
        Assert.Equal(1, tier.VagasOcupadas);
    }

    [Fact]
    public void OcuparVaga_QuandoEsgotado_DeveLancarDomainException()
    {
        var tier = CriarTier(limiteVagas: 1);
        tier.OcuparVaga();
        Assert.Throws<DomainException>(() => tier.OcuparVaga());
    }

    [Fact]
    public void TemVagasDisponiveis_QuandoAbaixoDoLimite_DeveRetornarTrue()
    {
        var tier = CriarTier(limiteVagas: 5);
        Assert.True(tier.TemVagasDisponiveis());
    }

    [Fact]
    public void TemVagasDisponiveis_QuandoEsgotado_DeveRetornarFalse()
    {
        var tier = CriarTier(limiteVagas: 1);
        tier.OcuparVaga();
        Assert.False(tier.TemVagasDisponiveis());
    }
}
