using Microsoft.EntityFrameworkCore;
using StarForge.Domain.Entities;
using StarForge.Infrastructure.Data.Configurations;

namespace StarForge.Infrastructure.Data;

public class StarForgeDbContext(DbContextOptions<StarForgeDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Missao> Missoes => Set<Missao>();
    public DbSet<Tier> Tiers => Set<Tier>();
    public DbSet<Nave> Naves => Set<Nave>();
    public DbSet<Contribuicao> Contribuicoes => Set<Contribuicao>();
    public DbSet<Hangar> Hangares => Set<Hangar>();
    public DbSet<FaseMissao> FasesMissao => Set<FaseMissao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
