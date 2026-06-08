using StarForge.Application.DTOs.Contribuicao;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.Application.Services;

public class ContribuicaoService(
    IContribuicaoRepository contribuicaoRepo,
    IUsuarioRepository usuarioRepo,
    IMissaoRepository missaoRepo,
    ITierRepository tierRepo,
    INaveRepository naveRepo,
    IHangarRepository hangarRepo) : IContribuicaoService
{
    public async Task<ContribuicaoDto> CriarAsync(CriarContribuicaoDto dto)
    {
        var missao = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        if (missao.Status != StatusMissao.Ativa)
            throw new BusinessRuleException("Só é possível contribuir para missões ativas.");

        var tier = await tierRepo.GetByIdAsync(dto.TierId)
            ?? throw new NotFoundException(nameof(Tier), dto.TierId);

        if (!tier.TemVagasDisponiveis())
            throw new BusinessRuleException("Não há vagas disponíveis neste tier.");

        _ = await usuarioRepo.GetByIdAsync(dto.UsuarioId)
            ?? throw new NotFoundException(nameof(Usuario), dto.UsuarioId);

        // Obtém a nave da missão para criar o registro no hangar
        var naves = await naveRepo.GetByMissaoIdAsync(dto.MissaoId);
        var nave = naves.FirstOrDefault()
            ?? throw new BusinessRuleException("A missão não possui naves cadastradas.");

        var contribuicao = new Contribuicao(dto.UsuarioId, dto.MissaoId, dto.TierId, dto.Valor, dto.MetodoPagamento);
        await contribuicaoRepo.AddAsync(contribuicao);

        var hangar = new Hangar(dto.UsuarioId, nave.Id, dto.MissaoId);
        await hangarRepo.AddAsync(hangar);

        await contribuicaoRepo.SaveChangesAsync();

        return MapToDto(contribuicao);
    }

    public async Task<ContribuicaoDto> ConfirmarAsync(Guid id)
    {
        var contribuicao = await contribuicaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Contribuicao), id);

        var usuario = await usuarioRepo.GetByIdAsync(contribuicao.UsuarioId)
            ?? throw new NotFoundException(nameof(Usuario), contribuicao.UsuarioId);

        var missao = await missaoRepo.GetByIdAsync(contribuicao.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), contribuicao.MissaoId);

        var tier = await tierRepo.GetByIdAsync(contribuicao.TierId)
            ?? throw new NotFoundException(nameof(Tier), contribuicao.TierId);

        // Regra de negócio crítica — ordem importa
        contribuicao.Confirmar();
        tier.OcuparVaga();
        usuario.AdicionarContribuicao(contribuicao.Valor);
        missao.RegistrarContribuicao(contribuicao.Valor);
        missao.VerificarMeta();

        usuarioRepo.Update(usuario);
        tierRepo.Update(tier);
        missaoRepo.Update(missao);
        contribuicaoRepo.Update(contribuicao);

        // Se missão concluída: desbloquear todos os hangares desta missão
        if (missao.Status == StatusMissao.Concluida)
        {
            var hangares = await hangarRepo.GetByMissaoIdAsync(missao.Id);
            foreach (var hangar in hangares.Where(h => h.Status == StatusHangar.Pendente))
            {
                hangar.Desbloquear();
                hangarRepo.Update(hangar);
            }
        }

        // Se missão falhou: cancelar contribuições pendentes (reembolso)
        if (missao.Status == StatusMissao.Falhou)
        {
            var pendentes = await contribuicaoRepo.GetByMissaoIdAsync(missao.Id);
            foreach (var c in pendentes.Where(c => c.Status == StatusContribuicao.Pendente))
            {
                c.Cancelar();
                contribuicaoRepo.Update(c);
            }
        }

        await contribuicaoRepo.SaveChangesAsync();
        return MapToDto(contribuicao);
    }

    public async Task<ContribuicaoDto> CancelarAsync(Guid id)
    {
        var contribuicao = await contribuicaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Contribuicao), id);

        contribuicao.Cancelar();
        contribuicaoRepo.Update(contribuicao);
        await contribuicaoRepo.SaveChangesAsync();

        return MapToDto(contribuicao);
    }

    public async Task<IEnumerable<ContribuicaoDto>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var contribuicoes = await contribuicaoRepo.GetByUsuarioIdAsync(usuarioId);
        return contribuicoes.Select(MapToDto);
    }

    private static ContribuicaoDto MapToDto(Contribuicao c) =>
        new(c.Id, c.UsuarioId, c.MissaoId, c.TierId, c.Valor, c.Status, c.MetodoPagamento,
            c.DataContribuicao, c.DataConfirmacao);
}
