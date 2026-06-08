using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StarForge.Infrastructure.Data;

/// <summary>
/// Fábrica de DbContext usada pelo EF Core Migrations em tempo de design.
/// Lê as credenciais do appsettings.Development.local.json da API.
/// </summary>
public class StarForgeDbContextFactory : IDesignTimeDbContextFactory<StarForgeDbContext>
{
    public StarForgeDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "StarForge.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.local.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<StarForgeDbContext>();
        optionsBuilder.UseOracle(config.GetConnectionString("DefaultConnection"));

        return new StarForgeDbContext(optionsBuilder.Options);
    }
}
