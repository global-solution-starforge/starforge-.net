namespace StarForge.Domain.Entities;

/// <summary>
/// Representa o jogador/piloto cadastrado na plataforma StarForge.
/// O nível é atualizado automaticamente conforme o total contribuído aumenta.
/// </summary>
public class Usuario
{
    /// <summary>Identificador único do usuário (chave primária).</summary>
    public Guid Id { get; private set; }

    /// <summary>Nome completo do piloto.</summary>
    public string Nome { get; private set; }

    /// <summary>Email de acesso à plataforma. Deve ser único.</summary>
    public string Email { get; private set; }

    /// <summary>RM do aluno FIAP. Deve ser único.</summary>
    public string Rm { get; private set; }

    /// <summary>Hash bcrypt da senha do usuário.</summary>
    public string SenhaHash { get; private set; }

    /// <summary>Indica se a conta está ativa.</summary>
    public bool Ativo { get; private set; }

    /// <summary>
    /// Nível do piloto calculado com base no total contribuído.
    /// RECRUTA menos de R$100, OPERATIVO R$100 a R$499,
    /// VETERANO R$500 a R$1999, COMANDANTE R$2000 ou mais.
    /// </summary>
    public string Nivel { get; private set; }

    /// <summary>Soma de todas as contribuições confirmadas do usuário.</summary>
    public decimal TotalContribuido { get; private set; }

    /// <summary>Data de criação da conta.</summary>
    public DateTime DataCadastro { get; private set; }

    /// <summary>Papel do usuário no sistema (User, Admin).</summary>
    public string Role { get; private set; }

    /// <summary>Contribuições realizadas por este usuário.</summary>
    public IReadOnlyCollection<Contribuicao> Contribuicoes => _contribuicoes.AsReadOnly();
    private readonly List<Contribuicao> _contribuicoes = [];

    /// <summary>Construtor privado reservado ao EF Core.</summary>
    private Usuario()
    {
        Nome = null!;
        Email = null!;
        Rm = null!;
        SenhaHash = null!;
        Nivel = null!;
        Role = null!;
    }

    /// <summary>Cria um novo usuário com os dados obrigatórios.</summary>
    public Usuario(string nome, string email, string rm, string senhaHash, string role = "User")
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Rm = rm;
        SenhaHash = senhaHash;
        Ativo = true;
        Nivel = "RECRUTA";
        TotalContribuido = 0;
        DataCadastro = DateTime.UtcNow;
        Role = role;
    }

    /// <summary>Atualiza os dados de perfil do usuário.</summary>
    public void AtualizarDados(string nome, string email)
    {
        Nome = nome;
        Email = email;
    }

    /// <summary>Substitui o hash da senha pelo novo valor gerado.</summary>
    public void AtualizarSenha(string novoHash) => SenhaHash = novoHash;

    /// <summary>Desativa a conta do usuário (soft delete).</summary>
    public void Desativar() => Ativo = false;

    /// <summary>Acumula o valor de uma contribuição confirmada e recalcula o nível.</summary>
    public void AdicionarContribuicao(decimal valor)
    {
        TotalContribuido += valor;
        AtualizarNivel();
    }

    /// <summary>Recalcula o nível do piloto com base no total contribuído acumulado.</summary>
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
