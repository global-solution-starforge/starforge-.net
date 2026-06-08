namespace StarForge.Domain.Enums;

/// <summary>
/// Representa o estado de execução de uma fase dentro de uma missão espacial.
/// </summary>
public enum StatusFaseMissao
{
    /// <summary>Fase cadastrada, aguardando início.</summary>
    Pendente = 1,

    /// <summary>Fase atualmente em execução.</summary>
    EmAndamento = 2,

    /// <summary>Fase finalizada com sucesso.</summary>
    Concluida = 3
}
