using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_TIER</c> no Oracle.
/// Define os mapeamentos de tipo, o relacionamento com <see cref="Missao"/> e o índice de consulta.
/// </summary>
public class TierConfiguration : IEntityTypeConfiguration<Tier>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="Tier"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<Tier> builder)
    {
        builder.ToTable("TB_TIER");

        // Chave primária com conversão Guid ↔ CHAR(36)
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // FK para missão — CHAR(36) espelha o tipo da PK de TB_MISSAO
        builder.Property(t => t.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Dados do tier
        builder.Property(t => t.Nome).HasColumnType("VARCHAR2(50)").IsRequired();
        builder.Property(t => t.Valor).HasColumnType("NUMBER(18,2)").IsRequired();           // Valor mínimo para entrar no tier
        builder.Property(t => t.BeneficioDescricao).HasColumnType("VARCHAR2(255)").IsRequired();
        builder.Property(t => t.LimiteVagas).HasColumnType("NUMBER(10)");                    // 0 = ilimitado
        builder.Property(t => t.VagasOcupadas).HasColumnType("NUMBER(10)");

        // Relacionamento explícito: um Tier pertence a uma Missão (CASCADE — tier sem missão não faz sentido)
        builder.HasOne<Missao>(t => t.Missao)
            .WithMany(m => m.Tiers)
            .HasForeignKey(t => t.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice para acelerar consultas de tiers por missão (GET /api/tiers?missaoId=xxx)
        builder.HasIndex(t => t.MissaoId)
            .HasDatabaseName("IX_TB_TIER_MISSAO_ID");
    }
}
