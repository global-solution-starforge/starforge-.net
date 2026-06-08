using StarForge.Application.DTOs.Nave;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

public class NaveService(INaveRepository naveRepo, IMissaoRepository missaoRepo) : INaveService
{
    public async Task<NaveDto> CriarAsync(CriarNaveDto dto)
    {
        _ = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        var nave = new Nave(dto.Nome, dto.Modelo, dto.Descricao, dto.Raridade, dto.MissaoId, dto.ImagemUrl);
        await naveRepo.AddAsync(nave);
        await naveRepo.SaveChangesAsync();
        return MapToDto(nave);
    }

    public async Task<NaveDto> GetByIdAsync(Guid id)
    {
        var nave = await naveRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Nave), id);
        return MapToDto(nave);
    }

    public async Task<IEnumerable<NaveDto>> GetByMissaoIdAsync(Guid missaoId)
    {
        var naves = await naveRepo.GetByMissaoIdAsync(missaoId);
        return naves.Select(MapToDto);
    }

    public async Task DeletarAsync(Guid id)
    {
        var nave = await naveRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Nave), id);
        naveRepo.Delete(nave);
        await naveRepo.SaveChangesAsync();
    }

    private static NaveDto MapToDto(Nave n) =>
        new(n.Id, n.Nome, n.Modelo, n.Descricao, n.Raridade, n.ImagemUrl, n.MissaoId);
}
