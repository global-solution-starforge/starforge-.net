using StarForge.Application.DTOs.Hangar;
using StarForge.Application.DTOs.Nave;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de consulta do hangar do usuário.
/// Carrega os hangares e enriquece cada um com os dados da nave correspondente.
/// </summary>
/// <param name="hangarRepo">Repositório de hangares.</param>
/// <param name="naveRepo">Repositório de naves — usado para buscar detalhes de cada nave.</param>
public class HangarService(IHangarRepository hangarRepo, INaveRepository naveRepo) : IHangarService
{
    /// <inheritdoc />
    public async Task<IEnumerable<HangarDto>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var hangares = await hangarRepo.GetByUsuarioIdAsync(usuarioId);
        var dtos = new List<HangarDto>();

        foreach (var h in hangares)
        {
            // Carrega os dados da nave para cada hangar individualmente
            var nave = await naveRepo.GetByIdAsync(h.NaveId);

            // Fallback defensivo: se a nave não for encontrada, retorna dados mínimos
            var naveDto = nave is null
                ? new NaveDto(h.NaveId, "Desconhecida", "", "", "", null, h.MissaoId)
                : new NaveDto(nave.Id, nave.Nome, nave.Modelo, nave.Descricao, nave.Raridade, nave.ImagemUrl, nave.MissaoId);

            dtos.Add(new HangarDto(h.Id, h.UsuarioId, h.MissaoId, h.Status, h.DataAquisicao, naveDto));
        }

        return dtos;
    }

    /// <inheritdoc />
    public async Task<HangarDto> GetByIdAsync(Guid id)
    {
        var hangar = await hangarRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Hangar), id);

        var nave = await naveRepo.GetByIdAsync(hangar.NaveId);
        var naveDto = nave is null
            ? new NaveDto(hangar.NaveId, "Desconhecida", "", "", "", null, hangar.MissaoId)
            : new NaveDto(nave.Id, nave.Nome, nave.Modelo, nave.Descricao, nave.Raridade, nave.ImagemUrl, nave.MissaoId);

        return new HangarDto(hangar.Id, hangar.UsuarioId, hangar.MissaoId, hangar.Status, hangar.DataAquisicao, naveDto);
    }
}
