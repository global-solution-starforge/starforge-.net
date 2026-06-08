namespace StarForge.Domain.Enums;

/// <summary>
/// Representa a disponibilidade de uma nave no hangar do jogador.
/// </summary>
/// <remarks>
/// Uma nave entra no hangar como <c>Pendente</c> quando o jogador confirma uma contribuição.
/// Torna-se <c>Desbloqueada</c> quando a missão associada atinge sua meta de arrecadação.
/// </remarks>
public enum StatusHangar
{
    /// <summary>Nave reservada; aguardando a conclusão da missão.</summary>
    Pendente = 1,

    /// <summary>Nave desbloqueada; missão concluída com sucesso.</summary>
    Desbloqueada = 2
}
