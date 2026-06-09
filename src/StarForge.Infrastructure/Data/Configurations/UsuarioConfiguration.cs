using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarForge.Domain.Entities;

namespace StarForge.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração Fluent API da tabela <c>TB_USUARIO</c> no Oracle.
/// Define os mapeamentos de tipo, restrições e índices únicos para a entidade <see cref="Usuario"/>.
/// </summary>
/// <remarks>
/// Mapeamentos Oracle notáveis:
/// <list type="bullet">
///   <item><c>CHAR(36)</c> para <see cref="Guid"/> — Oracle não tem tipo UUID nativo; armazenamos como string de 36 caracteres (ex: "550e8400-e29b-41d4-a716-446655440000")</item>
///   <item><c>NUMBER(1)</c> para <see cref="bool"/> — Oracle não tem tipo BOOLEAN em SQL; usamos 1=true / 0=false</item>
///   <item><c>TIMESTAMP</c> para <see cref="DateTime"/> — preserva microsegundos, ao contrário de DATE que só guarda segundos</item>
///   <item><c>NUMBER(18,2)</c> para <see cref="decimal"/> — 18 dígitos significativos, 2 casas decimais (adequado para valores monetários)</item>
/// </list>
/// </remarks>
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    /// <summary>
    /// Aplica as regras de mapeamento da entidade <see cref="Usuario"/> para o banco Oracle.
    /// </summary>
    /// <param name="builder">Construtor fluente fornecido pelo EF Core.</param>
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Nome da tabela Oracle com prefixo TB_ padrão do projeto
        builder.ToTable("TB_USUARIO");

        // Chave primária com conversão Guid ↔ CHAR(36) — Oracle não tem UUID nativo
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnType("CHAR(36)")
            .HasConversion(
                v => v.ToString(),           // Guid → string ao gravar
                v => Guid.Parse(v));         // string → Guid ao ler

        // Dados pessoais — todos VARCHAR2 com tamanhos máximos controlados
        builder.Property(u => u.Nome).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(u => u.Email).HasColumnType("VARCHAR2(100)").IsRequired();
        builder.Property(u => u.SenhaHash).HasColumnType("VARCHAR2(255)").IsRequired();  // Hash BCrypt tem ~60 chars, mas 255 dá margem
        builder.Property(u => u.Nivel).HasColumnType("VARCHAR2(20)").IsRequired();       // Iniciante, Explorador, Veterano, Elite
        builder.Property(u => u.Role).HasColumnType("VARCHAR2(20)").IsRequired();        // User ou Admin

        // Valor acumulado de contribuições — NUMBER(18,2) para precisão monetária
        builder.Property(u => u.TotalContribuido).HasColumnType("NUMBER(18,2)");

        // TIMESTAMP preserva fuso horário e microsegundos para rastreabilidade
        builder.Property(u => u.DataCadastro).HasColumnType("TIMESTAMP");

        // Oracle não tem BOOLEAN — conversão explícita 1=true / 0=false
        builder.Property(u => u.Ativo)
            .HasColumnType("NUMBER(1)")
            .HasConversion(v => v ? 1 : 0, v => v == 1);

        // Índice único para garantir unicidade de e-mail em nível de banco
        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_USUARIO_EMAIL");

        // Um usuário pode ter muitas contribuições; Restrict evita exclusão de usuário com histórico
        builder.HasMany(u => u.Contribuicoes)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
