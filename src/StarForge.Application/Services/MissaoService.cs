using StarForge.Application.DTOs.Missao;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de gerenciamento de missões de crowdfunding.
/// </summary>
/// <param name="missaoRepo">Repositório de missões.</param>
public class MissaoService(IMissaoRepository missaoRepo) : IMissaoService
{
    /// <inheritdoc />
    public async Task<MissaoDto> CriarAsync(CriarMissaoDto dto)
    {
        // DataInicio deve ser anterior à DataLimite — caso contrário a missão já começa inválida
        if (dto.DataInicio >= dto.DataLimite)
            throw new BusinessRuleException("A data de início deve ser anterior à data limite.");

        var missao = new Missao(dto.Nome, dto.Descricao, dto.Meta, dto.DataInicio, dto.DataLimite, dto.ImagemUrl);
        await missaoRepo.AddAsync(missao);
        await missaoRepo.SaveChangesAsync();
        return MapToDto(missao);
    }

    /// <inheritdoc />
    public async Task<MissaoDto> GetByIdAsync(Guid id)
    {
        var missao = await missaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Missao), id);
        return MapToDto(missao);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MissaoDto>> GetAllAsync()
    {
        var missoes = await missaoRepo.GetAllAsync();
        return missoes.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MissaoDto>> GetAtivasAsync()
    {
        var missoes = await missaoRepo.GetByStatusAsync(StatusMissao.Ativa);
        return missoes.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<MissaoDto> AtualizarAsync(Guid id, CriarMissaoDto dto)
    {
        var missao = await missaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Missao), id);

        missao.Atualizar(dto.Nome, dto.Descricao, dto.Meta, dto.DataLimite, dto.ImagemUrl);
        missaoRepo.Update(missao);
        await missaoRepo.SaveChangesAsync();
        return MapToDto(missao);
    }

    /// <inheritdoc />
    public async Task DeletarAsync(Guid id)
    {
        var missao = await missaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Missao), id);
        missaoRepo.Delete(missao);
        await missaoRepo.SaveChangesAsync();
    }

    /// <summary>
    /// Converte a entidade <see cref="Missao"/> para DTO de resposta,
    /// calculando o percentual arrecadado em relação à meta.
    /// </summary>
    private static MissaoDto MapToDto(Missao m)
    {
        // Evita divisão por zero se a meta for 0 (situação inválida, mas defensivo)
        var percentual = m.Meta > 0 ? Math.Round(m.TotalArrecadado / m.Meta * 100, 2) : 0;
        return new(m.Id, m.Nome, m.Descricao, m.Meta, m.TotalArrecadado, percentual,
                   m.DataInicio, m.DataLimite, m.Status, m.ImagemUrl);
    }
}
