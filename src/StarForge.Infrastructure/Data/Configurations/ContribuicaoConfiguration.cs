using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class ContribuicaoConfiguration : IEntityTypeConfiguration<Contribuicao>
{
    public void Configure(EntityTypeBuilder<Contribuicao> builder)
    {
        builder.ToTable("TB_CONTRIBUICAO");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(c => c.UsuarioId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(c => c.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(c => c.TierId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(c => c.Valor).HasColumnType("NUMBER(18,2)").IsRequired();
        builder.Property(c => c.Status).HasColumnType("NUMBER(2)");
        builder.Property(c => c.MetodoPagamento).HasColumnType("NUMBER(2)");
        builder.Property(c => c.DataContribuicao).HasColumnType("TIMESTAMP");
        builder.Property(c => c.DataConfirmacao).HasColumnType("TIMESTAMP");

        builder.HasOne(c => c.Tier)
            .WithMany()
            .HasForeignKey(c => c.TierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
