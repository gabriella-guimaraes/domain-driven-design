namespace ContainRs.Clientes.Cadastro;

public class Cliente
{
    private Cliente() { } // EF Core

    /// <summary>
    /// Pessoa física ou jurídica que solicita e contrata a locação de contêineres para uso próprio ou empresarial.
    /// </summary>
    /// <param name="nome"></param>
    /// <param name="email"></param>
    /// <param name="cPF"></param>
    public Cliente(string nome, Email email, string cPF)
    {
        Nome = nome;
        Email = email;
        CPF = cPF;
    }

    public Guid Id { get; set; }
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public string CPF { get; private set; }
    public string? Celular { get; set; }
    public ICollection<Endereco> Enderecos { get; set; }

    public Endereco AddEndereco(Endereco endereco)
    {
        Enderecos ??= [];
        Enderecos.Add(endereco);
        return endereco;
    }

    public void RemoveEndereco(Endereco endereco)
    {
        Enderecos.Remove(endereco);
    }

    public Endereco AddEndereco(string cep, string rua, string? numero, string? complemento, string? bairro, string municipio, UnidadeFederativa? estado)
    {
        var endereco = new Endereco
        {
            CEP = cep,
            Rua = rua,
            Numero = numero,
            Complemento = complemento,
            Bairro = bairro,
            Municipio = municipio,
            Estado = estado
        };
        return AddEndereco(endereco);
    }
}
