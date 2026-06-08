using StarForge.Domain.Enums;
using StarForge.Domain.Exceptions;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma fase/etapa narrativa de uma missão espacial.
/// Fases são exibidas em ordem crescente para mostrar o progresso da missão.
/// </summary>
/// <remarks>
/// Cada missão pode ter múltiplas fases ordenadas pelo campo <see cref="Ordem"/>.
/// A conclusão de uma fase é definitiva: não pode ser revertida.
/// </remarks>
public class FaseMissao
{
    /// <summary>Identificador único da fase (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>FK da missão à qual esta fase pertence.</summary>
    public Guid MissaoId { get; private set; }

    /// <summary>Título de exibição da fase (máx. 100 caracteres).</summary>
    public string Titulo { get; private set; }

    /// <summary>Descrição detalhada do que ocorre nesta fase (máx. 500 caracteres).</summary>
    public string Descricao { get; private set; }

    /// <summary>
    /// Número de sequência para ordenação das fases dentro da missão.
    /// Fases com menor valor de <see cref="Ordem"/> são exibidas primeiro.
    /// </summary>
    public int Ordem { get; private set; }

    /// <summary>Status atual desta fase no seu ciclo de vida.</summary>
    public StatusFaseMissao Status { get; private set; }

    /// <summary>Data e hora (UTC) em que a fase foi concluída. <c>null</c> se ainda não concluída.</summary>
    public DateTime? DataConclusao { get; private set; }

    /// <summary>Referência de navegação para a missão proprietária.</summary>
    public Missao Missao { get; private set; } = null!;

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core.
    /// </summary>
    private FaseMissao()
    {
        Titulo = null!;
        Descricao = null!;
    }

    /// <summary>
    /// Cria uma nova fase em status <see cref="StatusFaseMissao.Pendente"/>.
    /// </summary>
    /// <param name="missaoId">ID da missão à qual esta fase pertence.</param>
    /// <param name="titulo">Título da fase (máx. 100 caracteres).</param>
    /// <param name="descricao">Descrição da fase (máx. 500 caracteres).</param>
    /// <param name="ordem">Número de sequência para ordenação (1, 2, 3...).</param>
    public FaseMissao(Guid missaoId, string titulo, string descricao, int ordem)
    {
        Id = Guid.NewGuid();
        MissaoId = missaoId;
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
        Status = StatusFaseMissao.Pendente; // Toda fase começa pendente
    }

    /// <summary>
    /// Marca a fase como concluída e registra o momento da conclusão.
    /// Operação irreversível — uma fase concluída não pode ser reaberta.
    /// </summary>
    /// <exception cref="DomainException">
    /// Lançada se a fase já foi concluída anteriormente.
    /// </exception>
    public void Concluir()
    {
        if (Status == StatusFaseMissao.Concluida)
            throw new DomainException("Esta fase já foi concluída.");

        Status = StatusFaseMissao.Concluida;
        DataConclusao = DateTime.UtcNow; // Registra o timestamp exato da conclusão
    }

    /// <summary>
    /// Atualiza os dados editáveis da fase (título, descrição e ordem de exibição).
    /// </summary>
    /// <param name="titulo">Novo título da fase.</param>
    /// <param name="descricao">Nova descrição.</param>
    /// <param name="ordem">Nova posição na sequência de fases.</param>
    public void Atualizar(string titulo, string descricao, int ordem)
    {
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
    }
}
