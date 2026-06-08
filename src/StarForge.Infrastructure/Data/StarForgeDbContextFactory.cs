using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StarForge.Infrastructure.Data;

/// <summary>
/// Fábrica de <see cref="StarForgeDbContext"/> usada exclusivamente pelo EF Core Tools
/// durante operações de design-time: <c>dotnet ef migrations add</c> e <c>dotnet ef database update</c>.
/// </summary>
/// <remarks>
/// Esta classe NÃO é utilizada em runtime (nem em testes). Ela existe apenas para que o EF Core Tools
/// consiga instanciar o DbContext sem precisar inicializar toda a aplicação (<c>Program.cs</c>).
/// <para>
/// Para gerar/aplicar migrations, execute a partir da raiz do repositório:
/// <code>
/// dotnet ef migrations add NomeDaMigration --project src/StarForge.Infrastructure --startup-project src/StarForge.API
/// dotnet ef database update --project src/StarForge.Infrastructure --startup-project src/StarForge.API
/// </code>
/// Preencha <c>src/StarForge.API/appsettings.Development.local.json</c> com as credenciais Oracle antes de rodar.
/// </para>
/// </remarks>
public class StarForgeDbContextFactory : IDesignTimeDbContextFactory<StarForgeDbContext>
{
    /// <summary>
    /// Cria uma instância do DbContext lendo a connection string do projeto API.
    /// </summary>
    /// <param name="args">Argumentos de linha de comando (não utilizados).</param>
    /// <returns>Instância configurada de <see cref="StarForgeDbContext"/>.</returns>
    public StarForgeDbContext CreateDbContext(string[] args)
    {
        // Navega até a pasta do projeto API para ler os appsettings com as credenciais
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "StarForge.API"))
            .AddJsonFile("appsettings.json", optional: false)              // Base sem credenciais
            .AddJsonFile("appsettings.Development.local.json", optional: true) // Credenciais reais (gitignored)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<StarForgeDbContext>();
        optionsBuilder.UseOracle(config.GetConnectionString("DefaultConnection"));

        return new StarForgeDbContext(optionsBuilder.Options);
    }
}
