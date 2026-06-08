using StarForge.Application.DTOs.FaseMissao;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

public class FaseMissaoService(IFaseMissaoRepository faseRepo, IMissaoRepository missaoRepo) : IFaseMissaoService
{
    public async Task<FaseMissaoDto> CriarAsync(CriarFaseMissaoDto dto)
    {
        _ = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        var fase = new FaseMissao(dto.MissaoId, dto.Titulo, dto.Descricao, dto.Ordem);
        await faseRepo.AddAsync(fase);
        await faseRepo.SaveChangesAsync();
        return MapToDto(fase);
    }

    public async Task<FaseMissaoDto> GetByIdAsync(Guid id)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);
        return MapToDto(fase);
    }

    public async Task<IEnumerable<FaseMissaoDto>> GetByMissaoIdAsync(Guid missaoId)
    {
        var fases = await faseRepo.GetByMissaoIdAsync(missaoId);
        return fases.OrderBy(f => f.Ordem).Select(MapToDto);
    }

    public async Task<FaseMissaoDto> AtualizarAsync(Guid id, AtualizarFaseMissaoDto dto)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);

        fase.Atualizar(dto.Titulo, dto.Descricao, dto.Ordem);
        faseRepo.Update(fase);
        await faseRepo.SaveChangesAsync();
        return MapToDto(fase);
    }

    public async Task<FaseMissaoDto> ConcluirAsync(Guid id)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);

        fase.Concluir();
        faseRepo.Update(fase);
        await faseRepo.SaveChangesAsync();
        return MapToDto(fase);
    }

    public async Task DeletarAsync(Guid id)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);
        faseRepo.Delete(fase);
        await faseRepo.SaveChangesAsync();
    }

    private static FaseMissaoDto MapToDto(FaseMissao f) =>
        new(f.Id, f.MissaoId, f.Titulo, f.Descricao, f.Ordem, f.Status, f.DataConclusao);
}
