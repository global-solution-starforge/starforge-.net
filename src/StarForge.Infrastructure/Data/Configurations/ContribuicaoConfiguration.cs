using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_CONTRIBUICAO</c> no Oracle.
/// Define os mapeamentos de tipo e os relacionamentos com <see cref="Usuario"/>, <see cref="Missao"/> e <see cref="Tier"/>.
/// </summary>
/// <remarks>
/// Os relacionamentos <c>UsuarioId</c> e <c>MissaoId</c> são configurados em
/// <c>UsuarioConfiguration</c> e <c>MissaoConfiguration</c> respectivamente (pois são o lado "um" da relação).
/// Aqui configuramos apenas o relacionamento com <c>Tier</c>, que é o lado "muitos" sem navigation reversa em Tier.
/// </remarks>
public class ContribuicaoConfiguration : IEntityTypeConfiguration<Contribuicao>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="Contribuicao"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<Contribuicao> builder)
    {
        builder.ToTable("TB_CONTRIBUICAO");

        // Chave primária com conversão Guid ↔ CHAR(36)
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // FKs — CHAR(36) espelha o tipo das PKs referenciadas
        builder.Property(c => c.UsuarioId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(c => c.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(c => c.TierId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Valor financeiro — NUMBER(18,2) para precisão monetária
        builder.Property(c => c.Valor).HasColumnType("NUMBER(18,2)").IsRequired();

        // Enums armazenados como inteiros — Pendente=1, Confirmada=2, Cancelada=3, Reembolso=4
        builder.Property(c => c.Status).HasColumnType("NUMBER(2)");
        // MetodoPagamento: Pix=1, CartaoCredito=2, CartaoDebito=3, Boleto=4
        builder.Property(c => c.MetodoPagamento).HasColumnType("NUMBER(2)");

        // Datas — TIMESTAMP para precisão de microsegundos
        builder.Property(c => c.DataContribuicao).HasColumnType("TIMESTAMP");
        builder.Property(c => c.DataConfirmacao).HasColumnType("TIMESTAMP");  // null até confirmação

        // Restrict: deletar um Tier não deve apagar o histórico de contribuições feitas nele
        builder.HasOne(c => c.Tier)
            .WithMany()
            .HasForeignKey(c => c.TierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
