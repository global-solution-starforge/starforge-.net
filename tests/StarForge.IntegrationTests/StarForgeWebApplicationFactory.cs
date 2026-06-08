using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarForge.Infrastructure.Data;

namespace StarForge.IntegrationTests;

public class StarForgeWebApplicationFactory : WebApplicationFactory<Program>
{
    // Nome fixo por instância de factory — todos os requests e seeds compartilham o mesmo banco
    private readonly string _dbName = "StarForgeTestDb_" + Guid.NewGuid();

    // Provider isolado só com InMemory para evitar conflito Oracle/InMemory no EF Core
    private readonly IServiceProvider _inMemoryProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove DbContextOptions<StarForgeDbContext> registrado com Oracle
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<StarForgeDbContext>))
                .ToList();
            foreach (var d in descriptors)
                services.Remove(d);

            services.AddDbContext<StarForgeDbContext>(options =>
                options.UseInMemoryDatabase(_dbName)
                       .UseInternalServiceProvider(_inMemoryProvider));
        });
    }
}
