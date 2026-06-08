using StarForge.Application.DTOs.Usuario;

namespace StarForge.Application.Interfaces.Services;

/// <summary>
/// Serviço responsável pelo ciclo de vida dos usuários (cadastro, consulta, atualização, desativação).
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Cria um novo usuário validando unicidade de e-mail e RM.
    /// </summary>
    /// <param name="dto">Dados para criação da conta.</param>
    /// <returns>DTO com os dados públicos do usuário criado.</returns>
    /// <exception cref="Exceptions.BusinessRuleException">
    /// Lançada se o e-mail ou o RM já estiverem cadastrados.
    /// </exception>
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto);

    /// <summary>Retorna os dados de um usuário específico pelo ID.</summary>
    /// <exception cref="Exceptions.NotFoundException">Lançada se o usuário não existir.</exception>
    Task<UsuarioDto> GetByIdAsync(Guid id);

    /// <summary>Retorna todos os usuários cadastrados na plataforma.</summary>
    Task<IEnumerable<UsuarioDto>> GetAllAsync();

    /// <summary>
    /// Atualiza nome e e-mail de um usuário existente.
    /// </summary>
    /// <param name="id">ID do usuário a atualizar.</param>
    /// <param name="dto">Novos dados de perfil.</param>
    /// <returns>DTO com os dados atualizados.</returns>
    /// <exception cref="Exceptions.NotFoundException">Lançada se o usuário não existir.</exception>
    /// <exception cref="Exceptions.BusinessRuleException">Lançada se o novo e-mail já estiver em uso.</exception>
    Task<UsuarioDto> AtualizarAsync(Guid id, AtualizarUsuarioDto dto);

    /// <summary>
    /// Desativa (soft delete) a conta de um usuário. O registro é preservado no banco.
    /// </summary>
    /// <param name="id">ID do usuário a desativar.</param>
    /// <exception cref="Exceptions.NotFoundException">Lançada se o usuário não existir.</exception>
    Task DesativarAsync(Guid id);
}
