namespace StarForge.Domain.Enums;

/// <summary>
/// Representa o estado do pagamento de uma contribuição feita por um jogador a uma missão.
/// </summary>
/// <remarks>
/// O ciclo típico é <c>Pendente → Confirmado</c>. O estado <c>Reembolso</c> é aplicado
/// automaticamente quando a missão encerra com <see cref="StatusMissao.Falhou"/>.
/// </remarks>
public enum StatusContribuicao
{
    /// <summary>Pagamento iniciado, aguardando confirmação do gateway de pagamento.</summary>
    Pendente = 1,

    /// <summary>Pagamento confirmado; a contribuição é contabilizada na meta da missão.</summary>
    Confirmado = 2,

    /// <summary>Valor devolvido ao jogador em razão do encerramento sem sucesso da missão.</summary>
    Reembolso = 3
}
