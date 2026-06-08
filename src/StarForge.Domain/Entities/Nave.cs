namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma nave espacial associada a uma missão de crowdfunding.
/// É a recompensa principal que o contribuidor recebe no hangar ao concluir a missão.
/// </summary>
/// <remarks>
/// Cada missão possui ao menos uma nave cadastrada. Quando a missão é concluída com sucesso,
/// os hangares de todos os contribuidores que escolheram esta missão são desbloqueados,
/// revelando a nave conquistada.
/// </remarks>
public class Nave
{
    /// <summary>Identificador único da nave (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>Nome de exibição da nave (ex.: "Aurora Prime", "Nebula Destroyer").</summary>
    public string Nome { get; private set; }

    /// <summary>Modelo ou classe técnica da nave (ex.: "Classe A", "Interceptador").</summary>
    public string Modelo { get; private set; }

    /// <summary>Descrição narrativa e técnica da nave (máx. 500 caracteres).</summary>
    public string Descricao { get; private set; }

    /// <summary>
    /// Raridade da nave como string descritiva (ex.: "Comum", "Raro", "Épico", "Lendário").
    /// </summary>
    public string Raridade { get; private set; }

    /// <summary>URL opcional da imagem de arte da nave.</summary>
    public string? ImagemUrl { get; private set; }

    /// <summary>FK da missão à qual esta nave pertence como recompensa.</summary>
    public Guid MissaoId { get; private set; }

    /// <summary>Referência de navegação para a missão proprietária.</summary>
    public Missao Missao { get; private set; } = null!;

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core.
    /// </summary>
    private Nave()
    {
        Nome = null!;
        Modelo = null!;
        Descricao = null!;
        Raridade = null!;
    }

    /// <summary>
    /// Cria uma nova nave associada a uma missão específica.
    /// </summary>
    /// <param name="nome">Nome da nave (máx. 100 caracteres).</param>
    /// <param name="modelo">Modelo/classe da nave (máx. 50 caracteres).</param>
    /// <param name="descricao">Descrição narrativa (máx. 500 caracteres).</param>
    /// <param name="raridade">Raridade da nave (máx. 20 caracteres).</param>
    /// <param name="missaoId">ID da missão à qual a nave pertence.</param>
    /// <param name="imagemUrl">URL opcional da imagem de arte.</param>
    public Nave(string nome, string modelo, string descricao, string raridade, Guid missaoId, string? imagemUrl = null)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Modelo = modelo;
        Descricao = descricao;
        Raridade = raridade;
        MissaoId = missaoId;
        ImagemUrl = imagemUrl;
    }
}
