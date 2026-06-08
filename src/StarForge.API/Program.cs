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

var builder = WebApplication.CreateBuilder(args);

// Carrega appsettings.Development.local.json se existir
builder.Configuration.AddJsonFile("appsettings.Development.local.json", optional: true, reloadOnChange: true);

// DbContext Oracle
builder.Services.AddDbContext<StarForgeDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMissaoRepository, MissaoRepository>();
builder.Services.AddScoped<ITierRepository, TierRepository>();
builder.Services.AddScoped<INaveRepository, NaveRepository>();
builder.Services.AddScoped<IContribuicaoRepository, ContribuicaoRepository>();
builder.Services.AddScoped<IHangarRepository, HangarRepository>();
builder.Services.AddScoped<IFaseMissaoRepository, FaseMissaoRepository>();

// Serviços
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMissaoService, MissaoService>();
builder.Services.AddScoped<ITierService, TierService>();
builder.Services.AddScoped<INaveService, NaveService>();
builder.Services.AddScoped<IContribuicaoService, ContribuicaoService>();
builder.Services.AddScoped<IHangarService, HangarService>();
builder.Services.AddScoped<IFaseMissaoService, FaseMissaoService>();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? "StarForge@FallbackKey256BitsForDevelopment!!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// OpenAPI + Scalar
builder.Services.AddOpenApi();

var app = builder.Build();

// Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// OpenAPI / Scalar (DeepSpace)
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("StarForge API")
           .WithTheme(ScalarTheme.DeepSpace)
           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
