using Microsoft.EntityFrameworkCore;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data.Configurations;

namespace StarForge.Infrastructure.Data;

/// <summary>
/// Contexto principal do EF Core para o projeto StarForge.
/// Gerencia o acesso ao banco Oracle e aplica todas as configurações de mapeamento.
/// </summary>
/// <remarks>
/// Registrado como Scoped no DI container — uma instância por requisição HTTP.
/// Em produção usa Oracle; em testes usa InMemory (substituído pela <c>StarForgeWebApplicationFactory</c>).
/// </remarks>
/// <param name="options">Opções de configuração injetadas pelo DI (provider, connection string etc.).</param>
public class StarForgeDbContext(DbContextOptions<StarForgeDbContext> options) : DbContext(options)
{
    /// <summary>Tabela TB_USUARIO — usuários/pilotos cadastrados.</summary>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>Tabela TB_MISSAO — missões de crowdfunding.</summary>
    public DbSet<Missao> Missoes => Set<Missao>();

    /// <summary>Tabela TB_TIER — níveis de recompensa por missão.</summary>
    public DbSet<Tier> Tiers => Set<Tier>();

    /// <summary>Tabela TB_NAVE — naves espaciais como recompensas.</summary>
    public DbSet<Nave> Naves => Set<Nave>();

    /// <summary>Tabela TB_CONTRIBUICAO — contribuições financeiras dos usuários.</summary>
    public DbSet<Contribuicao> Contribuicoes => Set<Contribuicao>();

    /// <summary>Tabela TB_HANGAR — naves desbloqueadas ou pendentes no hangar do usuário.</summary>
    public DbSet<Hangar> Hangares => Set<Hangar>();

    /// <summary>Tabela TB_FASE_MISSAO — fases narrativas de cada missão.</summary>
    public DbSet<FaseMissao> FasesMissao => Set<FaseMissao>();

    /// <summary>
    /// Aplica as configurações Fluent API de todas as entidades.
    /// Cada entidade tem seu próprio arquivo de configuração com mapeamentos Oracle específicos.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica um arquivo de configuração por entidade (padrão IEntityTypeConfiguration<T>)
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new MissaoConfiguration());
        modelBuilder.ApplyConfiguration(new TierConfiguration());
        modelBuilder.ApplyConfiguration(new NaveConfiguration());
        modelBuilder.ApplyConfiguration(new ContribuicaoConfiguration());
        modelBuilder.ApplyConfiguration(new HangarConfiguration());
        modelBuilder.ApplyConfiguration(new FaseMissaoConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
