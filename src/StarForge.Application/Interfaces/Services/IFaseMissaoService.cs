using StarForge.Application.DTOs.FaseMissao;

namespace StarForge.Application.Interfaces.Services;

public interface IFaseMissaoService
{
    Task<FaseMissaoDto> CriarAsync(CriarFaseMissaoDto dto);
    Task<FaseMissaoDto> GetByIdAsync(Guid id);
    Task<IEnumerable<FaseMissaoDto>> GetByMissaoIdAsync(Guid missaoId);
    Task<FaseMissaoDto> AtualizarAsync(Guid id, AtualizarFaseMissaoDto dto);
    Task<FaseMissaoDto> ConcluirAsync(Guid id);
    Task DeletarAsync(Guid id);
}
