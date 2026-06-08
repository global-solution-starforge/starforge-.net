namespace StarForge.Domain.Enums;

/// <summary>
/// Representa o estado do pagamento de uma contribuição feita por um jogador a uma missão.
/// </summary>
public enum StatusContribuicao
{
    /// <summary>Pagamento iniciado, aguardando confirmação.</summary>
    Pendente = 1,

    /// <summary>Pagamento confirmado; a contribuição é contabilizada na meta da missão.</summary>
    Confirmada = 2,

    /// <summary>Contribuição cancelada pelo usuário.</summary>
    Cancelada = 3,

    /// <summary>Valor devolvido ao jogador em razão do encerramento sem sucesso da missão.</summary>
    Reembolso = 4
}
