using StarForge.Domain.Enums;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma missão espacial de crowdfunding.
/// Arrecada contribuições até atingir a meta dentro do prazo.
/// </summary>
public class Missao
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Meta { get; private set; }
    public decimal TotalArrecadado { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataLimite { get; private set; }
    public StatusMissao Status { get; private set; }
    public string? ImagemUrl { get; private set; }

    public IReadOnlyCollection<Tier> Tiers => _tiers.AsReadOnly();
    private readonly List<Tier> _tiers = [];

    public IReadOnlyCollection<Contribuicao> Contribuicoes => _contribuicoes.AsReadOnly();
    private readonly List<Contribuicao> _contribuicoes = [];

    public IReadOnlyCollection<FaseMissao> Fases => _fases.AsReadOnly();
    private readonly List<FaseMissao> _fases = [];

    private Missao()
    {
        Nome = null!;
        Descricao = null!;
    }

    public Missao(string nome, string descricao, decimal meta, DateTime dataInicio, DateTime dataLimite, string? imagemUrl = null)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        Meta = meta;
        TotalArrecadado = 0;
        DataInicio = dataInicio;
        DataLimite = dataLimite;
        Status = StatusMissao.Ativa;
        ImagemUrl = imagemUrl;
    }

    public void RegistrarContribuicao(decimal valor)
    {
        TotalArrecadado += valor;
    }

    /// <summary>
    /// Verifica se a missão atingiu a meta ou se o prazo expirou.
    /// Deve ser chamado após cada contribuição confirmada.
    /// </summary>
    public void VerificarMeta()
    {
        if (Status != StatusMissao.Ativa) return;

        if (TotalArrecadado >= Meta)
        {
            Status = StatusMissao.Concluida;
            return;
        }

        if (DateTime.UtcNow > DataLimite)
            Status = StatusMissao.Falhou;
    }

    public void Atualizar(string nome, string descricao, decimal meta, DateTime dataLimite, string? imagemUrl)
    {
        Nome = nome;
        Descricao = descricao;
        Meta = meta;
        DataLimite = dataLimite;
        ImagemUrl = imagemUrl;
    }
}
