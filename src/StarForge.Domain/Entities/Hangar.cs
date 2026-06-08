using StarForge.Domain.Enums;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa a posse de uma nave espacial no hangar de um usuário.
/// Criado com status <see cref="StatusHangar.Pendente"/> quando a contribuição é registrada
/// e desbloqueado automaticamente quando a missão é concluída com sucesso.
/// </summary>
/// <remarks>
/// Fluxo de um hangar:
/// <list type="number">
///   <item>Criado junto com a contribuição em <c>ContribuicaoService.CriarAsync</c> — status <c>Pendente</c>.</item>
///   <item>Desbloqueado em <c>ContribuicaoService.ConfirmarAsync</c> quando a missão atinge a meta — status <c>Desbloqueada</c>.</item>
///   <item>Removido se a contribuição for cancelada antes da confirmação.</item>
/// </list>
/// </remarks>
public class Hangar
{
    /// <summary>Identificador único do registro de hangar (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>FK do usuário dono desta nave.</summary>
    public Guid UsuarioId { get; private set; }

    /// <summary>FK da nave contida neste hangar.</summary>
    public Guid NaveId { get; private set; }

    /// <summary>FK da missão que originou esta nave.</summary>
    public Guid MissaoId { get; private set; }

    /// <summary>Status atual: <c>Pendente</c> (missão em andamento) ou <c>Desbloqueada</c> (missão concluída).</summary>
    public StatusHangar Status { get; private set; }

    /// <summary>Data e hora (UTC) em que o registro de hangar foi criado.</summary>
    public DateTime DataAquisicao { get; private set; }

    /// <summary>Referência de navegação para o usuário dono.</summary>
    public Usuario Usuario { get; private set; } = null!;

    /// <summary>Referência de navegação para a nave.</summary>
    public Nave Nave { get; private set; } = null!;

    /// <summary>Referência de navegação para a missão de origem.</summary>
    public Missao Missao { get; private set; } = null!;

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core.
    /// </summary>
    private Hangar()
    {
    }

    /// <summary>
    /// Cria um novo registro de hangar com status <see cref="StatusHangar.Pendente"/>.
    /// </summary>
    /// <param name="usuarioId">ID do usuário que receberá a nave.</param>
    /// <param name="naveId">ID da nave que será adicionada ao hangar.</param>
    /// <param name="missaoId">ID da missão que originou esta nave.</param>
    public Hangar(Guid usuarioId, Guid naveId, Guid missaoId)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        NaveId = naveId;
        MissaoId = missaoId;
        Status = StatusHangar.Pendente; // Aguarda conclusão da missão para ser desbloqueado
        DataAquisicao = DateTime.UtcNow;
    }

    /// <summary>
    /// Desbloqueia a nave no hangar após a missão ser concluída com sucesso.
    /// Chamado por <c>ContribuicaoService.ConfirmarAsync</c> quando a meta é atingida.
    /// </summary>
    public void Desbloquear() => Status = StatusHangar.Desbloqueada;
}
