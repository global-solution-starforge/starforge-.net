using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class MissaoConfiguration : IEntityTypeConfiguration<Missao>
{
    public void Configure(EntityTypeBuilder<Missao> builder)
    {
        builder.ToTable("TB_MISSAO");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(m => m.Nome).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(m => m.Descricao).HasColumnType("VARCHAR2(1000)").IsRequired();
        builder.Property(m => m.Meta).HasColumnType("NUMBER(18,2)").IsRequired();
        builder.Property(m => m.TotalArrecadado).HasColumnType("NUMBER(18,2)");
        builder.Property(m => m.DataInicio).HasColumnType("TIMESTAMP");
        builder.Property(m => m.DataLimite).HasColumnType("TIMESTAMP");
        builder.Property(m => m.Status).HasColumnType("NUMBER(2)");
        builder.Property(m => m.ImagemUrl).HasColumnType("VARCHAR2(255)");

        builder.HasMany(m => m.Tiers)
            .WithOne(t => t.Missao)
            .HasForeignKey(t => t.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Fases)
            .WithOne(f => f.Missao)
            .HasForeignKey(f => f.MissaoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Contribuicoes)
            .WithOne(c => c.Missao)
            .HasForeignKey(c => c.MissaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
