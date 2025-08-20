namespace ContainRs.Vendas.Propostas;

public record SituacaoProposta(string Status)
{
    public static SituacaoProposta Enviada => new("Enviada");
    public static SituacaoProposta Expirada => new("Expirada");
    public static SituacaoProposta Aceita => new("Aceita");
    public static SituacaoProposta Recusada => new("Recusada");
    public static SituacaoProposta Cancelada => new("Cancelada");

    public override string ToString() => Status;
    public static SituacaoProposta? Parse(string status)
    {
        return status switch
        {
            "Enviada" => Enviada,
            "Expirada" => Expirada,
            "Aceita" => Aceita,
            "Recusada" => Recusada,
            "Cancelada" => Cancelada,
            _ => null
        };
    }
}

/// <summary>
/// Documento ou oferta comercial apresentada ao cliente, contendo detalhes sobre os preços, condições de locação, prazos e especificações dos contêineres disponíveis.
/// </summary>

public class Proposta
{
    public Proposta() { }
    public Guid Id { get; set; }
    public SituacaoProposta Situacao { get; set; } = SituacaoProposta.Enviada;
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataExpiracao { get; set; }
    public string NomeArquivo { get; set; }
    public Guid ClienteId { get; set; }
    public Guid SolicitacaoId { get; set; }
    public PedidoLocacao Solicitacao { get; set; }
    public ICollection<Comentario> Comentarios { get; } = [];

    public Comentario AddComentario(Comentario comentario)
    {
        Comentarios.Add(comentario);
        return comentario;
    }

    public void RemoveComentario(Comentario comentario)
    {
        Comentarios.Remove(comentario);
    }
}
