namespace ContainRs.Domain.Models;

public class Endereco
{
    public Endereco() { }
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string CEP { get; set; }
    public string Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string Municipio { get; set; }
    public UnidadeFederativa? Estado { get; set; }
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; }
}
