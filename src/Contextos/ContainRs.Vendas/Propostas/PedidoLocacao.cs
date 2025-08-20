using ContainRs.Domain.Models;

namespace ContainRs.Vendas.Propostas;

public record StatusPedido(string Status)
{
    public static StatusPedido Ativa => new("Ativa");
    public static StatusPedido Inativa => new("Inativa");
    public static StatusPedido Cancelada => new("Cancelada");

    public override string ToString() => Status;
    public static StatusPedido? Parse(string status)
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

/// <summary>
/// Pedido formal realizado por um cliente interessado na locação de um contêiner. A solicitação pode incluir informações sobre finalidade, localização, quantidade e período desejado.
/// </summary>

public class PedidoLocacao
{
    public PedidoLocacao() { }

    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string Descricao { get; set; }
    public int QuantidadeEstimada { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Ativa;
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