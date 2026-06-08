using StarForge.Domain.Enums;
using StarForge.Domain.Exceptions;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma contribuição financeira de um usuário para uma missão.
/// </summary>
public class Contribuicao
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid MissaoId { get; private set; }
    public Guid TierId { get; private set; }
    public decimal Valor { get; private set; }
    public StatusContribuicao Status { get; private set; }
    public MetodoPagamento MetodoPagamento { get; private set; }
    public DateTime DataContribuicao { get; private set; }
    public DateTime? DataConfirmacao { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public Missao Missao { get; private set; } = null!;
    public Tier Tier { get; private set; } = null!;

    private Contribuicao()
    {
    }

    public Contribuicao(Guid usuarioId, Guid missaoId, Guid tierId, decimal valor, MetodoPagamento metodoPagamento)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        MissaoId = missaoId;
        TierId = tierId;
        Valor = valor;
        MetodoPagamento = metodoPagamento;
        Status = StatusContribuicao.Pendente;
        DataContribuicao = DateTime.UtcNow;
    }

    public void Confirmar()
    {
        if (Status != StatusContribuicao.Pendente)
            throw new DomainException("Apenas contribuições pendentes podem ser confirmadas.");

        Status = StatusContribuicao.Confirmada;
        DataConfirmacao = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        if (Status == StatusContribuicao.Cancelada)
            throw new DomainException("A contribuição já está cancelada.");

        Status = StatusContribuicao.Cancelada;
    }
}
