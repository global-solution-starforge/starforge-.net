using StarForge.Domain.Enums;

namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma missão espacial de crowdfunding na plataforma StarForge.
/// Uma missão arrecada contribuições dentro de um prazo para atingir uma meta financeira.
/// </summary>
/// <remarks>
/// Ciclo de vida de uma missão:
/// <list type="bullet">
///   <item><term>Ativa</term><description>Aceitando contribuições.</description></item>
///   <item><term>Concluida</term><description>Meta atingida antes do prazo.</description></item>
///   <item><term>Falhou</term><description>Prazo expirado sem atingir a meta.</description></item>
/// </list>
/// </remarks>
public class Missao
{
    /// <summary>Identificador único da missão (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>Nome de exibição da missão (máx. 100 caracteres).</summary>
    public string Nome { get; private set; }

    /// <summary>Descrição detalhada da missão (máx. 1000 caracteres).</summary>
    public string Descricao { get; private set; }

    /// <summary>Valor financeiro necessário para concluir a missão com sucesso.</summary>
    public decimal Meta { get; private set; }

    /// <summary>Soma de todas as contribuições confirmadas até o momento.</summary>
    public decimal TotalArrecadado { get; private set; }

    /// <summary>Data e hora (UTC) de início oficial da missão.</summary>
    public DateTime DataInicio { get; private set; }

    /// <summary>Data e hora (UTC) limite para atingir a meta. Após esta data, a missão falha.</summary>
    public DateTime DataLimite { get; private set; }

    /// <summary>Status atual da missão no ciclo de vida.</summary>
    public StatusMissao Status { get; private set; }

    /// <summary>URL opcional de imagem de capa da missão.</summary>
    public string? ImagemUrl { get; private set; }

    /// <summary>Tiers (níveis de recompensa) disponíveis nesta missão.</summary>
    public IReadOnlyCollection<Tier> Tiers => _tiers.AsReadOnly();
    private readonly List<Tier> _tiers = [];

    /// <summary>Contribuições recebidas para esta missão.</summary>
    public IReadOnlyCollection<Contribuicao> Contribuicoes => _contribuicoes.AsReadOnly();
    private readonly List<Contribuicao> _contribuicoes = [];

    /// <summary>Fases/etapas narrativas desta missão.</summary>
    public IReadOnlyCollection<FaseMissao> Fases => _fases.AsReadOnly();
    private readonly List<FaseMissao> _fases = [];

    /// <summary>Naves associadas como recompensa desta missão.</summary>
    public IReadOnlyCollection<Nave> Naves => _naves.AsReadOnly();
    private readonly List<Nave> _naves = [];

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core para materialização de entidades.
    /// </summary>
    private Missao()
    {
        Nome = null!;
        Descricao = null!;
    }

    /// <summary>
    /// Cria uma nova missão com status inicial <see cref="StatusMissao.Ativa"/>.
    /// </summary>
    /// <param name="nome">Nome da missão (máx. 100 caracteres).</param>
    /// <param name="descricao">Descrição detalhada (máx. 1000 caracteres).</param>
    /// <param name="meta">Valor financeiro a ser arrecadado. Deve ser maior que zero.</param>
    /// <param name="dataInicio">Data e hora (UTC) de início.</param>
    /// <param name="dataLimite">Data e hora (UTC) limite — deve ser posterior a <paramref name="dataInicio"/>.</param>
    /// <param name="imagemUrl">URL opcional da imagem de capa.</param>
    public Missao(string nome, string descricao, decimal meta, DateTime dataInicio, DateTime dataLimite, string? imagemUrl = null)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        Meta = meta;
        TotalArrecadado = 0;
        DataInicio = dataInicio;
        DataLimite = dataLimite;
        Status = StatusMissao.Ativa; // Toda missão começa ativa ao ser criada
        ImagemUrl = imagemUrl;
    }

    /// <summary>
    /// Incrementa o total arrecadado com o valor de uma contribuição confirmada.
    /// Deve ser chamado em conjunto com <see cref="VerificarMeta"/> após cada confirmação.
    /// </summary>
    /// <param name="valor">Valor da contribuição confirmada.</param>
    public void RegistrarContribuicao(decimal valor)
    {
        TotalArrecadado += valor;
    }

    /// <summary>
    /// Avalia o estado atual da missão após uma contribuição ser confirmada.
    /// Transita para <see cref="StatusMissao.Concluida"/> se a meta foi atingida,
    /// ou para <see cref="StatusMissao.Falhou"/> se o prazo expirou sem atingir a meta.
    /// </summary>
    /// <remarks>
    /// Idempotente: não altera o status caso a missão já esteja concluída ou tenha falhado.
    /// </remarks>
    public void VerificarMeta()
    {
        if (Status != StatusMissao.Ativa) return; // Só avalia missões ainda ativas

        if (TotalArrecadado >= Meta)
        {
            Status = StatusMissao.Concluida; // Meta atingida — missão concluída com sucesso
            return;
        }

        if (DateTime.UtcNow > DataLimite)
            Status = StatusMissao.Falhou; // Prazo expirado sem atingir a meta
    }

    /// <summary>
    /// Atualiza os dados editáveis da missão enquanto ainda está ativa.
    /// </summary>
    /// <param name="nome">Novo nome da missão.</param>
    /// <param name="descricao">Nova descrição.</param>
    /// <param name="meta">Nova meta financeira.</param>
    /// <param name="dataLimite">Novo prazo limite.</param>
    /// <param name="imagemUrl">Nova URL de imagem (pode ser <c>null</c>).</param>
    public void Atualizar(string nome, string descricao, decimal meta, DateTime dataLimite, string? imagemUrl)
    {
        Nome = nome;
        Descricao = descricao;
        Meta = meta;
        DataLimite = dataLimite;
        ImagemUrl = imagemUrl;
    }
}
