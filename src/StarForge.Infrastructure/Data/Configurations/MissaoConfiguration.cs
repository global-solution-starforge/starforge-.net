using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_MISSAO</c> no Oracle.
/// Define os mapeamentos de tipo, relacionamentos e cascades para a entidade <see cref="Missao"/>.
/// </summary>
/// <remarks>
/// <para>
/// A missão é o agregado central do sistema. Seus relacionamentos usam estratégias diferentes de exclusão:
/// </para>
/// <list type="bullet">
///   <item><c>Tiers</c> e <c>Fases</c> → CASCADE: não existem sem a missão</item>
///   <item><c>Contribuicoes</c> → RESTRICT: registros financeiros nunca devem ser excluídos em cascata (auditoria)</item>
/// </list>
/// </remarks>
public class MissaoConfiguration : IEntityTypeConfiguration<Missao>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="Missao"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<Missao> builder)
    {
        builder.ToTable("TB_MISSAO");

        // Chave primária com conversão Guid ↔ CHAR(36)
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Dados descritivos da missão
        builder.Property(m => m.Nome).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(m => m.Descricao).HasColumnType("VARCHAR2(1000)").IsRequired();

        // NUMBER(18,2) para valores monetários com precisão de centavos
        builder.Property(m => m.Meta).HasColumnType("NUMBER(18,2)").IsRequired();
        builder.Property(m => m.TotalArrecadado).HasColumnType("NUMBER(18,2)");

        // TIMESTAMP para datas com precisão de microsegundos
        builder.Property(m => m.DataInicio).HasColumnType("TIMESTAMP");
        builder.Property(m => m.DataLimite).HasColumnType("TIMESTAMP");

        // Enum armazenado como inteiro (NUMBER(2)): Ativa=1, Concluida=2, Falhou=3
        builder.Property(m => m.Status).HasColumnType("NUMBER(2)");

        // URL opcional da imagem de capa da missão
        builder.Property(m => m.ImagemUrl).HasColumnType("VARCHAR2(255)");

        // Tiers e Fases dependem da missão — exclusão em cascata é semânticamente correto
        builder.HasMany(m => m.Tiers)
            .WithOne(t => t.Missao)
            .HasForeignKey(t => t.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Fases)
            .WithOne(f => f.Missao)
            .HasForeignKey(f => f.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Contribuições são registros financeiros — Restrict para auditoria e integridade referencial
        builder.HasMany(m => m.Contribuicoes)
            .WithOne(c => c.Missao)
            .HasForeignKey(c => c.MissaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
