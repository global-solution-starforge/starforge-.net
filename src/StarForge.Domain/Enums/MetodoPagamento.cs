namespace StarForge.Domain.Enums;

/// <summary>
/// Define os meios de pagamento aceitos para processar contribuições na plataforma StarForge.
/// </summary>
/// <remarks>
/// O método de pagamento é registrado na tabela TB_CONTRIBUICAO e utilizado pelo
/// gateway para rotear a transação ao provedor correto.
/// </remarks>
public enum MetodoPagamento
{
    /// <summary>Pagamento instantâneo via Pix (Banco Central do Brasil).</summary>
    Pix = 1,

    /// <summary>Pagamento via cartão de crédito ou débito.</summary>
    Cartao = 2
}
