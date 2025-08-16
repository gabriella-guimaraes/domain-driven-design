using ContainRs.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace ContainRs.Api.Requests;

public class EnderecoRequest
{
    [Required]
    [Display(Prompt = "00.000-000")]
    public string CEP { get; set; }
    [Required]
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    [Required]
    public string? Municipio { get; set; }
    [Required]
    public string? Estado { get; set; }
    public string? Nome { get; set; }

    public Endereco ToModel() => new()
    {
        CEP = CEP,
        Rua = Rua!,
        Numero = Numero,
        Complemento = Complemento,
        Bairro = Bairro,
        Municipio = Municipio!,
        Estado = UfStringConverter.From(Estado),
        Nome = Nome
    };
}

public class RegistroRequest
{
    [Display(Name = "Nome (*)", Prompt = "Digite seu nome completo.")]
    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Nome { get; set; }

    [Display(Name = "E-mail (*)", Prompt = "Digite seu melhor e-mail.")]
    [Required(ErrorMessage = "Campo obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    [Display(Name = "CPF (*)", Prompt = "000.000.000-00")]
    [Required(ErrorMessage = "Campo obrigatório.")]
    public string CPF { get; set; }
    [Display(Prompt = "(00) 00000-0000")]
    public string? Celular { get; set; }
    public EnderecoRequest? Endereco { get; set; }
}
