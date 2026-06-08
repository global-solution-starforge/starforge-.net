using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class TierConfiguration : IEntityTypeConfiguration<Tier>
{
    public void Configure(EntityTypeBuilder<Tier> builder)
    {
        builder.ToTable("TB_TIER");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(t => t.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(t => t.Nome).HasColumnType("VARCHAR2(50)").IsRequired();
        builder.Property(t => t.Valor).HasColumnType("NUMBER(18,2)").IsRequired();
        builder.Property(t => t.BeneficioDescricao).HasColumnType("VARCHAR2(255)").IsRequired();
        builder.Property(t => t.LimiteVagas).HasColumnType("NUMBER(10)");
        builder.Property(t => t.VagasOcupadas).HasColumnType("NUMBER(10)");
    }
}
