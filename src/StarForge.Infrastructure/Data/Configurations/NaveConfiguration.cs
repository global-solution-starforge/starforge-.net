using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_NAVE</c> no Oracle.
/// Define os mapeamentos de tipo, o relacionamento com <see cref="Missao"/> e o índice de consulta.
/// </summary>
public class NaveConfiguration : IEntityTypeConfiguration<Nave>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="Nave"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<Nave> builder)
    {
        builder.ToTable("TB_NAVE");

        // Chave primária com conversão Guid ↔ CHAR(36)
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // FK para missão — CHAR(36) espelha o tipo da PK de TB_MISSAO
        builder.Property(n => n.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Dados da nave
        builder.Property(n => n.Nome).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(n => n.Modelo).HasColumnType("VARCHAR2(50)").IsRequired();
        builder.Property(n => n.Descricao).HasColumnType("VARCHAR2(500)").IsRequired();
        builder.Property(n => n.Raridade).HasColumnType("VARCHAR2(20)").IsRequired();  // Comum, Rara, Épica, Lendária
        builder.Property(n => n.ImagemUrl).HasColumnType("VARCHAR2(255)");

        // Relacionamento explícito: uma Nave pertence a uma Missão (CASCADE — nave sem missão não faz sentido)
        builder.HasOne<Missao>(n => n.Missao)
            .WithMany(m => m.Naves)
            .HasForeignKey(n => n.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice para acelerar consultas de naves por missão (GET /api/naves?missaoId=xxx)
        builder.HasIndex(n => n.MissaoId)
            .HasDatabaseName("IX_TB_NAVE_MISSAO_ID");
    }
}
