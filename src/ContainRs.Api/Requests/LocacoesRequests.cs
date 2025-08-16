using System.ComponentModel.DataAnnotations;

namespace ContainRs.Api.Requests;

public record ComentarioRequest([Required] string Comentario);

public class PropostaRequest
{
    [Required]
    public decimal ValorTotal { get; set; }
    [Required]
    public DateTime DataExpiracao { get; set; }
    [Required]
    public IFormFile Arquivo { get; set; }
}

    public class SolicitacaoRequest
{
    [Required]
    public string Finalidade { get; set; }
    public int QuantidadeEstimada { get; set; }
    [Required]
    public PeriodoRequest Periodo { get; set; }
    [Required]
    public LocalizacaoRequest Localizacao { get; set; }
    [Required]
    public string Descricao { get; set; }
}

public class LocalizacaoRequest
{
    public Guid? EnderecoId { get; set; }
    public string? CEP { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Municipio { get; set; }
    public string? Estado { get; set; }
}

public class PeriodoRequest
{
    public DateTime DataInicioOperacao { get; set; }
    public int DisponibilidadePrevia { get; set; }
    public int QuantidadeDias { get; set; }
}