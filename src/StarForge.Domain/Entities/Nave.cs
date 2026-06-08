namespace StarForge.Domain.Entities;

/// <summary>
/// Representa uma nave espacial associada a uma missão.
/// É a recompensa desbloqueada no hangar do contribuidor quando a missão é concluída.
/// </summary>
public class Nave
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Modelo { get; private set; }
    public string Descricao { get; private set; }
    public string Raridade { get; private set; }
    public string? ImagemUrl { get; private set; }
    public Guid MissaoId { get; private set; }

    public Missao Missao { get; private set; } = null!;

    private Nave()
    {
        Nome = null!;
        Modelo = null!;
        Descricao = null!;
        Raridade = null!;
    }

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
