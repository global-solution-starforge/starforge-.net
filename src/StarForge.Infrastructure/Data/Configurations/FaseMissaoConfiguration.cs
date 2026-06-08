using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_FASE_MISSAO</c> no Oracle.
/// Define os mapeamentos de tipo, o relacionamento com <see cref="Missao"/> e o índice de consulta.
/// </summary>
public class FaseMissaoConfiguration : IEntityTypeConfiguration<FaseMissao>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="FaseMissao"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<FaseMissao> builder)
    {
        builder.ToTable("TB_FASE_MISSAO");

        // Chave primária com conversão Guid ↔ CHAR(36)
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // FK para missão — CHAR(36) espelha o tipo da PK de TB_MISSAO
        builder.Property(f => f.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Dados narrativos da fase
        builder.Property(f => f.Titulo).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(f => f.Descricao).HasColumnType("VARCHAR2(500)").IsRequired();
        builder.Property(f => f.Ordem).HasColumnType("NUMBER(5)");                      // Posição sequencial dentro da missão

        // Enum StatusFaseMissao: Pendente=1, EmAndamento=2, Concluida=3 — NUMBER(2) para inteiros pequenos
        builder.Property(f => f.Status).HasColumnType("NUMBER(2)");

        // TIMESTAMP — null enquanto não concluída; preenchido por FaseMissao.Concluir()
        builder.Property(f => f.DataConclusao).HasColumnType("TIMESTAMP");

        // Relacionamento explícito: uma FaseMissao pertence a uma Missão (CASCADE — fase sem missão não faz sentido)
        builder.HasOne<Missao>()
            .WithMany(m => m.Fases)
            .HasForeignKey(f => f.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice para acelerar consultas de fases por missão e para ordenação por campo Ordem
        builder.HasIndex(f => f.MissaoId)
            .HasDatabaseName("IX_TB_FASE_MISSAO_MISSAO_ID");
    }
}
