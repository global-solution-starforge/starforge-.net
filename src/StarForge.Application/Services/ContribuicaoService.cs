using StarForge.Application.DTOs.Contribuicao;
using StarForge.Application.Exceptions;
using StarForge.Application.Interfaces;
using StarForge.Application.Interfaces.Services;
using StarForge.Domain.Entities;
using StarForge.Domain.Enums;

namespace StarForge.Application.Services;

/// <summary>
/// Implementação do serviço de contribuições — orquestra a cadeia de efeitos colaterais
/// entre Contribuicao, Tier, Usuario, Missao e Hangar.
/// </summary>
/// <remarks>
/// Este é o serviço mais complexo do sistema. O método <see cref="ConfirmarAsync"/> executa
/// uma sequência de operações que devem ser atômicas (todas persistidas em uma única transação).
/// </remarks>
public class ContribuicaoService(
    IContribuicaoRepository contribuicaoRepo,
    IUsuarioRepository usuarioRepo,
    IMissaoRepository missaoRepo,
    ITierRepository tierRepo,
    INaveRepository naveRepo,
    IHangarRepository hangarRepo) : IContribuicaoService
{
    /// <inheritdoc />
    public async Task<ContribuicaoDto> CriarAsync(CriarContribuicaoDto dto)
    {
        // Valida se a missão existe e está aceitando contribuições
        var missao = await missaoRepo.GetByIdAsync(dto.MissaoId)
            ?? throw new NotFoundException(nameof(Missao), dto.MissaoId);

        if (missao.Status != StatusMissao.Ativa)
            throw new BusinessRuleException("Só é possível contribuir para missões ativas.");

        // Valida disponibilidade de vagas no tier escolhido
        var tier = await tierRepo.GetByIdAsync(dto.TierId)
            ?? throw new NotFoundException(nameof(Tier), dto.TierId);

        if (!tier.TemVagasDisponiveis())
            throw new BusinessRuleException("Não há vagas disponíveis neste tier.");

        // Valida existência do usuário
        _ = await usuarioRepo.GetByIdAsync(dto.UsuarioId)
            ?? throw new NotFoundException(nameof(Usuario), dto.UsuarioId);

        // Obtém a primeira nave da missão para associar ao hangar (cada missão deve ter ao menos uma nave)
        var naves = await naveRepo.GetByMissaoIdAsync(dto.MissaoId);
        var nave = naves.FirstOrDefault()
            ?? throw new BusinessRuleException("A missão não possui naves cadastradas.");

        // Cria contribuição pendente — pagamento ainda não processado
        var contribuicao = new Contribuicao(dto.UsuarioId, dto.MissaoId, dto.TierId, dto.Valor, dto.MetodoPagamento);
        await contribuicaoRepo.AddAsync(contribuicao);

        // Reserva a nave no hangar do usuário — fica pendente até a missão ser concluída
        var hangar = new Hangar(dto.UsuarioId, nave.Id, dto.MissaoId);
        await hangarRepo.AddAsync(hangar);

        await contribuicaoRepo.SaveChangesAsync(); // Persiste contribuição + hangar atomicamente
        return MapToDto(contribuicao);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Sequência obrigatória de operações (ordem importa):
    /// <list type="number">
    ///   <item>Confirma o status da contribuição.</item>
    ///   <item>Ocupa a vaga no tier.</item>
    ///   <item>Acumula valor no total do usuário e recalcula seu nível.</item>
    ///   <item>Registra o valor arrecadado na missão.</item>
    ///   <item>Verifica se a meta foi atingida ou o prazo expirou.</item>
    ///   <item>Se concluída: desbloqueia todos os hangares desta missão.</item>
    ///   <item>Se falhou: marca todas as contribuições pendentes para reembolso.</item>
    /// </list>
    /// </remarks>
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

        // Regra de negócio crítica — ordem importa para consistência dos dados
        contribuicao.Confirmar();                              // 1. Confirma pagamento
        tier.OcuparVaga();                                     // 2. Reduz vagas disponíveis no tier
        usuario.AdicionarContribuicao(contribuicao.Valor);     // 3. Atualiza total e nível do piloto
        missao.RegistrarContribuicao(contribuicao.Valor);      // 4. Soma ao total arrecadado da missão
        missao.VerificarMeta();                                // 5. Avalia se meta foi atingida ou prazo expirou

        usuarioRepo.Update(usuario);
        tierRepo.Update(tier);
        missaoRepo.Update(missao);
        contribuicaoRepo.Update(contribuicao);

        // Se missão concluída: desbloqueia todos os hangares desta missão
        if (missao.Status == StatusMissao.Concluida)
        {
            var hangares = await hangarRepo.GetByMissaoIdAsync(missao.Id);
            foreach (var hangar in hangares.Where(h => h.Status == StatusHangar.Pendente))
            {
                hangar.Desbloquear(); // Revela a nave no hangar de cada contribuidor
                hangarRepo.Update(hangar);
            }
        }

        // Se missão falhou: marca contribuições pendentes para reembolso (não cancelamento voluntário)
        if (missao.Status == StatusMissao.Falhou)
        {
            var pendentes = await contribuicaoRepo.GetByMissaoIdAsync(missao.Id);
            foreach (var c in pendentes.Where(c => c.Status == StatusContribuicao.Pendente))
            {
                c.MarcarReembolso(); // Indica que o valor deve ser devolvido ao usuário
                contribuicaoRepo.Update(c);
            }
        }

        await contribuicaoRepo.SaveChangesAsync(); // Persiste todas as alterações em uma única operação
        return MapToDto(contribuicao);
    }

    /// <inheritdoc />
    public async Task<ContribuicaoDto> CancelarAsync(Guid id)
    {
        var contribuicao = await contribuicaoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Contribuicao), id);

        contribuicao.Cancelar();
        contribuicaoRepo.Update(contribuicao);

        // Remove o hangar pendente associado (criado junto com a contribuição) — evita registro órfão no banco
        var hangares = await hangarRepo.GetByUsuarioIdAsync(contribuicao.UsuarioId);
        var hangarPendente = hangares.FirstOrDefault(h =>
            h.MissaoId == contribuicao.MissaoId && h.Status == StatusHangar.Pendente);
        if (hangarPendente is not null)
            hangarRepo.Delete(hangarPendente);

        await contribuicaoRepo.SaveChangesAsync();
        return MapToDto(contribuicao);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ContribuicaoDto>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var contribuicoes = await contribuicaoRepo.GetByUsuarioIdAsync(usuarioId);
        return contribuicoes.Select(MapToDto);
    }

    /// <summary>Mapeia a entidade <see cref="Contribuicao"/> para o DTO de resposta.</summary>
    private static ContribuicaoDto MapToDto(Contribuicao c) =>
        new(c.Id, c.UsuarioId, c.MissaoId, c.TierId, c.Valor, c.Status, c.MetodoPagamento,
            c.DataContribuicao, c.DataConfirmacao);
}
