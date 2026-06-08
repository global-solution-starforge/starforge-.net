using StarForge.Application.DTOs.FaseMissao;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de gerenciamento das fases narrativas de missões.
/// </summary>
/// <param name="faseRepo">Repositório de fases de missão.</param>
/// <param name="missaoRepo">Repositório de missões — usado para validar existência antes de criar.</param>
public class FaseMissaoService(IFaseMissaoRepository faseRepo, IMissaoRepository missaoRepo) : IFaseMissaoService
{
    /// <inheritdoc />
    public async Task<FaseMissaoDto> CriarAsync(CriarFaseMissaoDto dto)
    {
        // Valida existência da missão — fase sem missão não pode existir
        _ = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        var fase = new FaseMissao(dto.MissaoId, dto.Titulo, dto.Descricao, dto.Ordem);
        await faseRepo.AddAsync(fase);
        await faseRepo.SaveChangesAsync();
        return MapToDto(fase);
    }

    /// <inheritdoc />
    public async Task<FaseMissaoDto> GetByIdAsync(Guid id)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);
        return MapToDto(fase);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FaseMissaoDto>> GetByMissaoIdAsync(Guid missaoId)
    {
        var fases = await faseRepo.GetByMissaoIdAsync(missaoId);
        return fases.OrderBy(f => f.Ordem).Select(MapToDto); // Ordena pela sequência narrativa
    }

    /// <inheritdoc />
    public async Task<FaseMissaoDto> AtualizarAsync(Guid id, AtualizarFaseMissaoDto dto)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);

        fase.Atualizar(dto.Titulo, dto.Descricao, dto.Ordem);
        faseRepo.Update(fase);
        await faseRepo.SaveChangesAsync();
        return MapToDto(fase);
    }

    /// <inheritdoc />
    public async Task<FaseMissaoDto> ConcluirAsync(Guid id)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);

        fase.Concluir(); // DomainException se já estiver concluída
        faseRepo.Update(fase);
        await faseRepo.SaveChangesAsync();
        return MapToDto(fase);
    }

    /// <inheritdoc />
    public async Task DeletarAsync(Guid id)
    {
        var fase = await faseRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(FaseMissao), id);
        faseRepo.Delete(fase);
        await faseRepo.SaveChangesAsync();
    }

    /// <summary>Mapeia a entidade <see cref="FaseMissao"/> para o DTO de resposta.</summary>
    private static FaseMissaoDto MapToDto(FaseMissao f) =>
        new(f.Id, f.MissaoId, f.Titulo, f.Descricao, f.Ordem, f.Status, f.DataConclusao);
}
