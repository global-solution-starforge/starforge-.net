using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class HangarConfiguration : IEntityTypeConfiguration<Hangar>
{
    public void Configure(EntityTypeBuilder<Hangar> builder)
    {
        builder.ToTable("TB_HANGAR");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(h => h.UsuarioId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(h => h.NaveId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(h => h.MissaoId)
            .HasColumnType("CHAR(36)")
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(h => h.Status).HasColumnType("NUMBER(2)");
        builder.Property(h => h.DataAquisicao).HasColumnType("TIMESTAMP");

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
