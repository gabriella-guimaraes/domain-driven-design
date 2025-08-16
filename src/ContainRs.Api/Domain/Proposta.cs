namespace ContainRs.Api.Domain;

public record StatusProposta(string Status)
{
    public static StatusProposta Enviada => new("Enviada");
    public static StatusProposta Expirada => new("Expirada");
    public static StatusProposta Aceita => new("Aceita");
    public static StatusProposta Recusada => new("Recusada");
    public override string ToString() => Status;
    public static StatusProposta? Parse(string status)
    {
        return status switch
        {
            "Enviada" => Enviada,
            "Expirada" => Expirada,
            "Aceita" => Aceita,
            "Recusada" => Recusada,
            _ => null
        };
    }
}

public class Proposta
{
    public Proposta() { }
    public Guid Id { get; set; }
    public StatusProposta Status { get; set; } = StatusProposta.Enviada;
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataExpiracao { get; set; }
    public string NomeArquivo { get; set; }
    public Guid ClienteId { get; set; }
    public Guid SolicitacaoId { get; set; }
    public Solicitacao Solicitacao { get; set; }
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
