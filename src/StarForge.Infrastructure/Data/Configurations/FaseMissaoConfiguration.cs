using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class FaseMissaoConfiguration : IEntityTypeConfiguration<FaseMissao>
{
    public void Configure(EntityTypeBuilder<FaseMissao> builder)
    {
        builder.ToTable("TB_FASE_MISSAO");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(f => f.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(f => f.Titulo).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(f => f.Descricao).HasColumnType("VARCHAR2(500)").IsRequired();
        builder.Property(f => f.Ordem).HasColumnType("NUMBER(5)");
        builder.Property(f => f.Status).HasColumnType("NUMBER(2)");
        builder.Property(f => f.DataConclusao).HasColumnType("TIMESTAMP");
    }
}
