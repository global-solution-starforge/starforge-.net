namespace StarForge.Application.DTOs.Usuario;

/// <summary>
/// DTO de resposta com os dados públicos de um usuário. Não expõe dados sensíveis como <c>SenhaHash</c>.
/// </summary>
public record UsuarioDto(
    /// <summary>Identificador único do usuário.</summary>
    Guid Id,

    /// <summary>Nome completo do piloto.</summary>
    string Nome,

    /// <summary>E-mail de acesso.</summary>
    string Email,

    /// <summary>Nível atual do piloto (RECRUTA, OPERATIVO, VETERANO ou COMANDANTE).</summary>
    string Nivel,

    /// <summary>Soma total de contribuições confirmadas em reais.</summary>
    decimal TotalContribuido,

    /// <summary>Indica se a conta está ativa. Contas desativadas não aparecem no sistema.</summary>
    bool Ativo,

    /// <summary>Papel no sistema (<c>"User"</c> ou <c>"Admin"</c>).</summary>
    string Role,

    /// <summary>Data e hora (UTC) de criação da conta.</summary>
    DateTime DataCadastro
);
