namespace StarForge.Domain.Enums;

/// <summary>
/// Representa o estado de execução de uma fase dentro de uma missão espacial.
/// </summary>
/// <remarks>
/// Cada missão é composta por múltiplas fases sequenciais (TB_FASE_MISSAO).
/// Uma fase só pode avançar para <c>EmAndamento</c> após a anterior ser <c>Concluida</c>.
/// </remarks>
public enum StatusFaseMissao
{
    /// <summary>Fase cadastrada, aguardando o início da fase anterior ser concluída.</summary>
    Aguardando = 1,

    /// <summary>Fase atualmente em execução pela equipe da missão.</summary>
    EmAndamento = 2,

    /// <summary>Fase finalizada com sucesso; próxima fase pode ser iniciada.</summary>
    Concluido = 3
}
