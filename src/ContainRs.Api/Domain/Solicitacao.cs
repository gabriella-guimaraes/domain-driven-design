using ContainRs.Domain.Models;

namespace ContainRs.Api.Domain;

public record StatusSolicitacao(string Status)
{
    public static StatusSolicitacao Ativa => new("Ativa");
    public static StatusSolicitacao Inativa => new("Inativa");
    public static StatusSolicitacao Cancelada => new("Cancelada");

    public override string ToString() => Status;
    public static StatusSolicitacao? Parse(string status)
    {
        return status switch
        {
            "Ativa" => Ativa,
            "Inativa" => Inativa,
            "Cancelada" => Cancelada,
            _ => null
        };
    }
}

public class Solicitacao
{
    public Solicitacao() { }

    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string Descricao { get; set; }
    public int QuantidadeEstimada { get; set; }
    public StatusSolicitacao Status { get; set; } = StatusSolicitacao.Ativa;
    public string Finalidade { get; set; }
    public DateTime DataInicioOperacao { get; set; }
    public int DisponibilidadePrevia { get; set; }
    public int DuracaoPrevistaLocacao { get; set; }
    public Guid EnderecoId { get; set; }
    public Endereco Localizacao { get; set; }

    public ICollection<Proposta> Propostas { get; } = [];

    public Proposta AddProposta(Proposta proposta)
    {
        Propostas.Add(proposta);
        return proposta;
    }

    public void RemoveProposta(Proposta proposta)
    {
        Propostas.Remove(proposta);
    }

}