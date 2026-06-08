using StarForge.Domain.Enums;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma nave desbloqueada (ou pendente) no hangar de um usuário.
/// Criado quando a contribuição é confirmada; desbloqueado quando a missão é concluída.
/// </summary>
public class Hangar
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid NaveId { get; private set; }
    public Guid MissaoId { get; private set; }
    public StatusHangar Status { get; private set; }
    public DateTime DataAquisicao { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public Nave Nave { get; private set; } = null!;
    public Missao Missao { get; private set; } = null!;

    private Hangar()
    {
    }

    public Hangar(Guid usuarioId, Guid naveId, Guid missaoId)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        NaveId = naveId;
        MissaoId = missaoId;
        Status = StatusHangar.Pendente;
        DataAquisicao = DateTime.UtcNow;
    }

    public void Desbloquear() => Status = StatusHangar.Desbloqueada;
}
