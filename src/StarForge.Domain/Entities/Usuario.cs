namespace StarForge.Domain.Entities;

/// <summary>
/// Representa o jogador/piloto cadastrado na plataforma StarForge.
/// </summary>
/// <remarks>
/// O nível é calculado automaticamente com base no total acumulado de contribuições confirmadas:
/// <list type="bullet">
///   <item><term>RECRUTA</term><description>Menos de R$ 100,00</description></item>
///   <item><term>OPERATIVO</term><description>De R$ 100,00 a R$ 499,99</description></item>
///   <item><term>VETERANO</term><description>De R$ 500,00 a R$ 1.999,99</description></item>
///   <item><term>COMANDANTE</term><description>R$ 2.000,00 ou mais</description></item>
/// </list>
/// </remarks>
public class Usuario
{
    /// <summary>Identificador único do usuário (chave primária, gerado automaticamente).</summary>
    public Guid Id { get; private set; }

    /// <summary>Nome completo do piloto exibido na plataforma.</summary>
    public string Nome { get; private set; }

    /// <summary>E-mail de acesso único na plataforma. Utilizado como login.</summary>
    public string Email { get; private set; }

    /// <summary>Hash BCrypt da senha. Nunca armazena senha em texto puro.</summary>
    public string SenhaHash { get; private set; }

    /// <summary>Indica se a conta está ativa. Contas desativadas não podem fazer login.</summary>
    public bool Ativo { get; private set; }

    /// <summary>
    /// Nível hierárquico do piloto, atualizado automaticamente por <see cref="AdicionarContribuicao"/>.
    /// Valores possíveis: RECRUTA, OPERATIVO, VETERANO, COMANDANTE.
    /// </summary>
    public string Nivel { get; private set; }

    /// <summary>Soma de todas as contribuições com status <c>Confirmada</c>.</summary>
    public decimal TotalContribuido { get; private set; }

    /// <summary>Data e hora (UTC) em que a conta foi criada.</summary>
    public DateTime DataCadastro { get; private set; }

    /// <summary>
    /// Papel do usuário para controle de acesso baseado em roles (RBAC).
    /// Valores esperados: <c>"User"</c> (padrão) ou <c>"Admin"</c>.
    /// </summary>
    public string Role { get; private set; }

    /// <summary>
    /// Coleção somente-leitura das contribuições deste usuário.
    /// Carregada pelo EF Core via lazy/eager loading quando necessário.
    /// </summary>
    public IReadOnlyCollection<Contribuicao> Contribuicoes => _contribuicoes.AsReadOnly();
    private readonly List<Contribuicao> _contribuicoes = [];

    /// <summary>
    /// Construtor privado sem parâmetros exigido pelo EF Core para materialização de entidades.
    /// Não utilizar diretamente no código da aplicação.
    /// </summary>
    private Usuario()
    {
        Nome = null!;
        Email = null!;
        SenhaHash = null!;
        Nivel = null!;
        Role = null!;
    }

    /// <summary>
    /// Cria um novo usuário com os dados obrigatórios.
    /// O nível inicial é sempre RECRUTA e a conta começa ativa.
    /// </summary>
    /// <param name="nome">Nome completo do piloto.</param>
    /// <param name="email">E-mail único de acesso.</param>
    /// <param name="senhaHash">Hash BCrypt da senha (gerado antes de chamar o construtor).</param>
    /// <param name="role">Papel no sistema. Padrão: <c>"User"</c>.</param>
    public Usuario(string nome, string email, string senhaHash, string role = "User")
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Ativo = true;
        Nivel = "RECRUTA";        // Todo piloto começa como recruta
        TotalContribuido = 0;
        DataCadastro = DateTime.UtcNow;
        Role = role;
    }

    /// <summary>
    /// Atualiza nome e e-mail do perfil do usuário.
    /// A unicidade do e-mail deve ser validada pelo serviço antes de chamar este método.
    /// </summary>
    /// <param name="nome">Novo nome completo.</param>
    /// <param name="email">Novo e-mail (deve ser único na base).</param>
    public void AtualizarDados(string nome, string email)
    {
        Nome = nome;
        Email = email;
    }

    /// <summary>
    /// Substitui o hash da senha atual pelo novo hash gerado.
    /// </summary>
    /// <param name="novoHash">Novo hash BCrypt da nova senha.</param>
    public void AtualizarSenha(string novoHash) => SenhaHash = novoHash;

    /// <summary>
    /// Desativa a conta do usuário (soft delete).
    /// Usuários desativados não conseguem fazer login, mas seus dados são preservados.
    /// </summary>
    public void Desativar() => Ativo = false;

    /// <summary>
    /// Registra uma contribuição confirmada, acumulando o valor no total e recalculando o nível.
    /// </summary>
    /// <param name="valor">Valor confirmado da contribuição (deve ser positivo).</param>
    public void AdicionarContribuicao(decimal valor)
    {
        TotalContribuido += valor;
        AtualizarNivel(); // Recalcula o nível após cada contribuição confirmada
    }

    /// <summary>
    /// Recalcula o nível hierárquico do piloto com base no total acumulado.
    /// Chamado automaticamente por <see cref="AdicionarContribuicao"/>.
    /// </summary>
    private void AtualizarNivel()
    {
        Nivel = TotalContribuido switch
        {
            >= 2000 => "COMANDANTE",
            >= 500  => "VETERANO",
            >= 100  => "OPERATIVO",
            _       => "RECRUTA"
        };
    }
}
