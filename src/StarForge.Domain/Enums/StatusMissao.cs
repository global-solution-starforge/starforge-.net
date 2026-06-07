namespace StarForge.Domain.Enums;

/// <summary>
/// Representa o ciclo de vida de uma missão espacial de crowdfunding.
/// </summary>
/// <remarks>
/// O estado de uma missão é gerenciado pelo sistema com base no progresso
/// de arrecadação e nas fases cadastradas. Transições permitidas:
/// <c>Ativa → Concluida</c> (meta atingida) ou <c>Ativa → Falhou</c> (prazo expirado sem meta).
/// </remarks>
public enum StatusMissao
{
    /// <summary>Missão em andamento, aceitando contribuições dos jogadores.</summary>
    Ativa = 1,

    /// <summary>Meta de arrecadação atingida; naves foram liberadas no hangar dos contribuidores.</summary>
    Concluida = 2,

    /// <summary>Prazo encerrado sem atingir a meta; contribuições devem ser reembolsadas.</summary>
    Falhou = 3
}
