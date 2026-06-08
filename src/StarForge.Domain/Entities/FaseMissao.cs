using StarForge.Domain.Enums;
using StarForge.Domain.Exceptions;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma fase/etapa dentro de uma missão espacial.
/// Fases são exibidas como progresso narrativo da missão.
/// </summary>
public class FaseMissao
{
    public Guid Id { get; private set; }
    public Guid MissaoId { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public int Ordem { get; private set; }
    public StatusFaseMissao Status { get; private set; }
    public DateTime? DataConclusao { get; private set; }

    public Missao Missao { get; private set; } = null!;

    private FaseMissao()
    {
        Titulo = null!;
        Descricao = null!;
    }

    public FaseMissao(Guid missaoId, string titulo, string descricao, int ordem)
    {
        Id = Guid.NewGuid();
        MissaoId = missaoId;
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
        Status = StatusFaseMissao.Pendente;
    }

    public void Concluir()
    {
        if (Status == StatusFaseMissao.Concluida)
            throw new DomainException("Esta fase já foi concluída.");

        Status = StatusFaseMissao.Concluida;
        DataConclusao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string descricao, int ordem)
    {
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
    }
}
