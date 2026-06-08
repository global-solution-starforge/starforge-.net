using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oracle.EntityFrameworkCore;
using Scalar.AspNetCore;
using StarForge.API.Middlewares;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Application.Services;
using StarForge.Infrastructure.Data;
using StarForge.Infrastructure.Repositories;

// ─────────────────────────────────────────────────────────────────────────────
// CONFIGURAÇÃO DO HOST E SERVIÇOS (DI Container)
// ─────────────────────────────────────────────────────────────────────────────
var builder = WebApplication.CreateBuilder(args);

// Carrega credenciais Oracle e chave JWT do arquivo local (gitignored).
// O arquivo appsettings.Development.local.json deve ser preenchido pelo desenvolvedor/professor
// com as credenciais reais antes de rodar as migrations ou a API.
builder.Configuration.AddJsonFile("appsettings.Development.local.json", optional: true, reloadOnChange: true);

// ── BANCO DE DADOS ───────────────────────────────────────────────────────────
// Registra o DbContext com o provider Oracle usando a connection string do appsettings.
// Scoped: uma instância de StarForgeDbContext por requisição HTTP.
builder.Services.AddDbContext<StarForgeDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── REPOSITÓRIOS (Scoped) ────────────────────────────────────────────────────
// Cada interface de repositório mapeia para sua implementação concreta.
// Scoped garante que todas as operações de uma requisição compartilhem o mesmo DbContext.
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMissaoRepository, MissaoRepository>();
builder.Services.AddScoped<ITierRepository, TierRepository>();
builder.Services.AddScoped<INaveRepository, NaveRepository>();
builder.Services.AddScoped<IContribuicaoRepository, ContribuicaoRepository>();
builder.Services.AddScoped<IHangarRepository, HangarRepository>();
builder.Services.AddScoped<IFaseMissaoRepository, FaseMissaoRepository>();

// ── SERVIÇOS DE APLICAÇÃO (Scoped) ───────────────────────────────────────────
// Camada de orquestração: recebe repositórios por injeção e contém as regras de negócio.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMissaoService, MissaoService>();
builder.Services.AddScoped<ITierService, TierService>();
builder.Services.AddScoped<INaveService, NaveService>();
builder.Services.AddScoped<IContribuicaoService, ContribuicaoService>();
builder.Services.AddScoped<IHangarService, HangarService>();
builder.Services.AddScoped<IFaseMissaoService, FaseMissaoService>();

// ── AUTENTICAÇÃO JWT BEARER ──────────────────────────────────────────────────
// Lê a chave secreta do appsettings; usa fallback para desenvolvimento se ausente.
// Em produção, a chave DEVE estar configurada via variável de ambiente ou secrets.
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? "StarForge@FallbackKey256BitsForDevelopment!!";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],         // "StarForge.API"

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],     // "StarForge.Client"

            ValidateLifetime = true,                                    // Rejeita tokens expirados
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ── DOCUMENTAÇÃO OPENAPI + SCALAR ────────────────────────────────────────────
// AddOpenApi() gera o schema OpenAPI 3.x automaticamente a partir dos controllers.
// Scalar é a UI de documentação interativa que substitui o Swagger UI.
builder.Services.AddOpenApi();

// ─────────────────────────────────────────────────────────────────────────────
// PIPELINE HTTP (Middlewares na ordem correta)
// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ExceptionHandlingMiddleware DEVE ser o primeiro — garante que qualquer exceção
// em qualquer outro middleware seja capturada e retorne ProblemDetails (RFC 7807).
app.UseMiddleware<ExceptionHandlingMiddleware>();

// RequestLoggingMiddleware depois do ExceptionHandling — assim loga o status
// code final (incluindo os 4xx/5xx gerados pelo tratamento de exceções).
app.UseMiddleware<RequestLoggingMiddleware>();

// Expõe o schema OpenAPI em /openapi/v1.json
app.MapOpenApi();

// UI Scalar com tema DeepSpace — acessível em /scalar/v1
app.MapScalarApiReference(options =>
{
    options.WithTitle("StarForge API")
           .WithTheme(ScalarTheme.DeepSpace)
           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

// UseAuthentication ANTES de UseAuthorization — autentica o usuário (preenche User.Claims)
// para que UseAuthorization possa avaliar os [Authorize] dos controllers.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Partial class necessária para que WebApplicationFactory<Program> nos testes de integração
// consiga referenciar o tipo Program do assembly StarForge.API.
public partial class Program { }
