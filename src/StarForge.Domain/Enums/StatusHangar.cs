namespace StarForge.Domain.Enums;

/// <summary>
/// Representa a disponibilidade de uma nave no hangar do jogador.
/// </summary>
/// <remarks>
/// Naves são adicionadas ao hangar (TB_HANGAR) quando a missão associada é concluída.
/// Uma nave <c>Bloqueada</c> indica que a missão ainda não atingiu sua meta ou
/// que a contribuição do jogador está com pagamento <c>Pendente</c>.
/// </remarks>
public enum StatusHangar
{
    /// <summary>Nave reservada no hangar; aguardando confirmação da missão ou do pagamento.</summary>
    Pendente = 1,

    /// <summary>Nave bloqueada; o jogador não pode utilizá-la até a condição ser resolvida.</summary>
    Bloqueado = 2
}
