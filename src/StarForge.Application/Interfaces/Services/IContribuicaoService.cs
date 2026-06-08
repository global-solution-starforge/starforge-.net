using StarForge.Application.DTOs.Contribuicao;

namespace StarForge.Application.Interfaces.Services;

public interface IContribuicaoService
{
    Task<ContribuicaoDto> CriarAsync(CriarContribuicaoDto dto);
    Task<ContribuicaoDto> ConfirmarAsync(Guid id);
    Task<ContribuicaoDto> CancelarAsync(Guid id);
    Task<IEnumerable<ContribuicaoDto>> GetByUsuarioIdAsync(Guid usuarioId);
}
