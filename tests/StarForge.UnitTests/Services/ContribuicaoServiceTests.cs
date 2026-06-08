using Moq;
using StarForge.Application.DTOs.Contribuicao;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Services;
using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.UnitTests.Services;

public class ContribuicaoServiceTests
{
    private readonly Mock<IContribuicaoRepository> _contribuicaoRepo = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IMissaoRepository> _missaoRepo = new();
    private readonly Mock<ITierRepository> _tierRepo = new();
    private readonly Mock<INaveRepository> _naveRepo = new();
    private readonly Mock<IHangarRepository> _hangarRepo = new();

    private ContribuicaoService CriarService() => new(
        _contribuicaoRepo.Object, _usuarioRepo.Object, _missaoRepo.Object,
        _tierRepo.Object, _naveRepo.Object, _hangarRepo.Object);

    [Fact]
    public async Task CriarAsync_MissaoNaoAtiva_DeveLancarBusinessRuleException()
    {
        var missao = new Missao("M", "D", 1000m, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-1));
        // forçar missão como Falhou via VerificarMeta
        missao.VerificarMeta();

        _missaoRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(missao);

        var service = CriarService();
        var dto = new CriarContribuicaoDto(Guid.NewGuid(), missao.Id, Guid.NewGuid(), 100m, MetodoPagamento.Pix);

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.CriarAsync(dto));
    }

    [Fact]
    public async Task ConfirmarAsync_DeveAtualizarNivelDoUsuario()
    {
        var usuario = new Usuario("Piloto", "piloto@fiap.com.br", "RM001", "hash");
        var missaoId = Guid.NewGuid();
        var tierId = Guid.NewGuid();
        var missao = new Missao("Missão X", "Desc", 10000m, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30));
        var tier = new Tier("Silver", 500m, "Nave prata", 10, missaoId);
        var contribuicao = new Contribuicao(usuario.Id, missaoId, tierId, 500m, MetodoPagamento.Pix);

        _contribuicaoRepo.Setup(r => r.GetByIdAsync(contribuicao.Id)).ReturnsAsync(contribuicao);
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
        _missaoRepo.Setup(r => r.GetByIdAsync(missaoId)).ReturnsAsync(missao);
        _tierRepo.Setup(r => r.GetByIdAsync(tierId)).ReturnsAsync(tier);
        _hangarRepo.Setup(r => r.GetByMissaoIdAsync(It.IsAny<Guid>())).ReturnsAsync([]);

        var service = CriarService();
        await service.ConfirmarAsync(contribuicao.Id);

        Assert.Equal("VETERANO", usuario.Nivel);
        Assert.Equal(500m, usuario.TotalContribuido);
    }

    [Fact]
    public async Task ConfirmarAsync_QuandoMetaAtingida_DeveDefinirMissaoComoConcluida()
    {
        var usuario = new Usuario("Piloto", "piloto@fiap.com.br", "RM001", "hash");
        var missaoId = Guid.NewGuid();
        var tierId = Guid.NewGuid();
        var missao = new Missao("Missão Y", "Desc", 100m, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30));
        var tier = new Tier("Gold", 100m, "Nave ouro", 10, missaoId);
        var contribuicao = new Contribuicao(usuario.Id, missaoId, tierId, 100m, MetodoPagamento.Cartao);

        _contribuicaoRepo.Setup(r => r.GetByIdAsync(contribuicao.Id)).ReturnsAsync(contribuicao);
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
        _missaoRepo.Setup(r => r.GetByIdAsync(missaoId)).ReturnsAsync(missao);
        _tierRepo.Setup(r => r.GetByIdAsync(tierId)).ReturnsAsync(tier);
        _hangarRepo.Setup(r => r.GetByMissaoIdAsync(It.IsAny<Guid>())).ReturnsAsync([]);

        var service = CriarService();
        await service.ConfirmarAsync(contribuicao.Id);

        Assert.Equal(StatusMissao.Concluida, missao.Status);
    }

    [Fact]
    public async Task ConfirmarAsync_QuandoMetaAtingida_DeveDesbloquearHangares()
    {
        var usuario = new Usuario("Piloto", "piloto@fiap.com.br", "RM001", "hash");
        var missaoId = Guid.NewGuid();
        var tierId = Guid.NewGuid();
        var naveId = Guid.NewGuid();
        var missao = new Missao("Missão Z", "Desc", 100m, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30));
        var tier = new Tier("Platinum", 100m, "Nave platina", 5, missaoId);
        var contribuicao = new Contribuicao(usuario.Id, missaoId, tierId, 100m, MetodoPagamento.Pix);
        var hangar = new Hangar(usuario.Id, naveId, missaoId);

        _contribuicaoRepo.Setup(r => r.GetByIdAsync(contribuicao.Id)).ReturnsAsync(contribuicao);
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
        _missaoRepo.Setup(r => r.GetByIdAsync(missaoId)).ReturnsAsync(missao);
        _tierRepo.Setup(r => r.GetByIdAsync(tierId)).ReturnsAsync(tier);
        _hangarRepo.Setup(r => r.GetByMissaoIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Hangar> { hangar });

        var service = CriarService();
        await service.ConfirmarAsync(contribuicao.Id);

        Assert.Equal(StatusHangar.Desbloqueada, hangar.Status);
    }
}
