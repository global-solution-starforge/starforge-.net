using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_HANGAR</c> no Oracle.
/// Define os mapeamentos de tipo e os relacionamentos com <see cref="Usuario"/>, <see cref="Nave"/> e <see cref="Missao"/>.
/// </summary>
/// <remarks>
/// O hangar registra qual nave um usuário ganhou (ou está aguardando) de qual missão.
/// Todos os relacionamentos usam <c>RESTRICT</c> porque o hangar é um registro histórico de
/// aquisição — não deve ser excluído em cascata ao deletar nave, usuário ou missão.
/// </remarks>
public class HangarConfiguration : IEntityTypeConfiguration<Hangar>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="Hangar"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<Hangar> builder)
    {
        builder.ToTable("TB_HANGAR");

        // Chave primária com conversão Guid ↔ CHAR(36)
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // FKs — CHAR(36) espelha o tipo das PKs referenciadas
        builder.Property(h => h.UsuarioId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(h => h.NaveId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(h => h.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Enum StatusHangar: Pendente=1, Desbloqueada=2 — NUMBER(2) para inteiros pequenos
        builder.Property(h => h.Status).HasColumnType("NUMBER(2)");

        // TIMESTAMP para rastreabilidade da data de aquisição
        builder.Property(h => h.DataAquisicao).HasColumnType("TIMESTAMP");

        // Restrict em todos os relacionamentos — hangar é registro histórico, não deve ser cascateado
        builder.HasOne(h => h.Usuario)
            .WithMany()
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.Nave)
            .WithMany()
            .HasForeignKey(h => h.NaveId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.Missao)
            .WithMany()
            .HasForeignKey(h => h.MissaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
