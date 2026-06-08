using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class NaveConfiguration : IEntityTypeConfiguration<Nave>
{
    public void Configure(EntityTypeBuilder<Nave> builder)
    {
        builder.ToTable("TB_NAVE");

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(n => n.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(n => n.Nome).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(n => n.Modelo).HasColumnType("VARCHAR2(50)").IsRequired();
        builder.Property(n => n.Descricao).HasColumnType("VARCHAR2(500)").IsRequired();
        builder.Property(n => n.Raridade).HasColumnType("VARCHAR2(20)").IsRequired();
        builder.Property(n => n.ImagemUrl).HasColumnType("VARCHAR2(255)");
    }
}
