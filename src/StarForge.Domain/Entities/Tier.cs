using StarForge.Domain.Exceptions;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa um nível de recompensa (tier) disponível em uma missão de crowdfunding.
/// Cada tier define um valor de contribuição e concede uma nave espacial ao contribuidor.
/// </summary>
/// <remarks>
/// Um tier possui vagas limitadas — quando todas estão ocupadas, não aceita mais contribuições.
/// A vaga é ocupada apenas quando a contribuição é confirmada (não na criação).
/// </remarks>
public class Tier
{
    /// <summary>Identificador único do tier (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>Nome de exibição do tier (ex.: "Pioneiro", "Comandante Elite").</summary>
    public string Nome { get; private set; }

    /// <summary>Valor em reais que o contribuidor deve pagar para obter este tier.</summary>
    public decimal Valor { get; private set; }

    /// <summary>Descrição dos benefícios e recompensas concedidos por este tier.</summary>
    public string BeneficioDescricao { get; private set; }

    /// <summary>Número máximo de vagas disponíveis neste tier.</summary>
    public int LimiteVagas { get; private set; }

    /// <summary>Número de vagas atualmente ocupadas (contribuições confirmadas).</summary>
    public int VagasOcupadas { get; private set; }

    /// <summary>FK da missão à qual este tier pertence.</summary>
    public Guid MissaoId { get; private set; }

    /// <summary>Referência de navegação para a missão proprietária.</summary>
    public Missao Missao { get; private set; } = null!;

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core.
    /// </summary>
    private Tier()
    {
        Nome = null!;
        BeneficioDescricao = null!;
    }

    /// <summary>
    /// Cria um novo tier associado a uma missão, com zero vagas ocupadas inicialmente.
    /// </summary>
    /// <param name="nome">Nome do tier (máx. 50 caracteres).</param>
    /// <param name="valor">Valor em reais para participar deste tier.</param>
    /// <param name="beneficioDescricao">Descrição dos benefícios (máx. 255 caracteres).</param>
    /// <param name="limiteVagas">Número máximo de vagas disponíveis.</param>
    /// <param name="missaoId">ID da missão à qual o tier pertence.</param>
    public Tier(string nome, decimal valor, string beneficioDescricao, int limiteVagas, Guid missaoId)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Valor = valor;
        BeneficioDescricao = beneficioDescricao;
        LimiteVagas = limiteVagas;
        VagasOcupadas = 0; // Nenhuma vaga ocupada no momento da criação
        MissaoId = missaoId;
    }

    /// <summary>
    /// Verifica se ainda há vagas disponíveis neste tier.
    /// </summary>
    /// <returns><c>true</c> se <see cref="VagasOcupadas"/> for menor que <see cref="LimiteVagas"/>.</returns>
    public bool TemVagasDisponiveis() => VagasOcupadas < LimiteVagas;

    /// <summary>
    /// Ocupa uma vaga neste tier ao confirmar uma contribuição.
    /// </summary>
    /// <exception cref="DomainException">
    /// Lançada quando todas as vagas já foram preenchidas e não é possível aceitar mais contribuições.
    /// </exception>
    public void OcuparVaga()
    {
        if (!TemVagasDisponiveis())
            throw new DomainException("Não há vagas disponíveis neste tier.");

        VagasOcupadas++; // Incrementa após validação para garantir consistência
    }
}
