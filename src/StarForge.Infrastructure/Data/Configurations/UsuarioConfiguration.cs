using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("TB_USUARIO");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(
                v => v.ToString(),
                v => Guid.Parse(v));

        builder.Property(u => u.Nome).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(u => u.Email).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(u => u.Rm).HasColumnType("VARCHAR2(20)").IsRequired();
        builder.Property(u => u.SenhaHash).HasColumnType("VARCHAR2(255)").IsRequired();
        builder.Property(u => u.Nivel).HasColumnType("VARCHAR2(20)").IsRequired();
        builder.Property(u => u.Role).HasColumnType("VARCHAR2(20)").IsRequired();
        builder.Property(u => u.TotalContribuido).HasColumnType("NUMBER(18,2)");
        builder.Property(u => u.DataCadastro).HasColumnType("TIMESTAMP");
        builder.Property(u => u.Ativo)
            .HasColumnType("NUMBER(1)")
            .HasConversion(v => v ? 1 : 0, v => v == 1);

        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_USUARIO_EMAIL");
        builder.HasIndex(u => u.Rm).IsUnique().HasDatabaseName("IX_USUARIO_RM");

        builder.HasMany(u => u.Contribuicoes)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
