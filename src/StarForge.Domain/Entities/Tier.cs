using StarForge.Domain.Exceptions;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa um nível de recompensa disponível em uma missão.
/// Cada tier define um valor mínimo de contribuição e uma nave exclusiva como recompensa.
/// </summary>
public class Tier
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public decimal Valor { get; private set; }
    public string BeneficioDescricao { get; private set; }
    public int LimiteVagas { get; private set; }
    public int VagasOcupadas { get; private set; }
    public Guid MissaoId { get; private set; }

    public Missao Missao { get; private set; } = null!;

    private Tier()
    {
        Nome = null!;
        BeneficioDescricao = null!;
    }

    public Tier(string nome, decimal valor, string beneficioDescricao, int limiteVagas, Guid missaoId)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Valor = valor;
        BeneficioDescricao = beneficioDescricao;
        LimiteVagas = limiteVagas;
        VagasOcupadas = 0;
        MissaoId = missaoId;
    }

    public bool TemVagasDisponiveis() => VagasOcupadas < LimiteVagas;

    public void OcuparVaga()
    {
        if (!TemVagasDisponiveis())
            throw new DomainException("Não há vagas disponíveis neste tier.");
        VagasOcupadas++;
    }
}
