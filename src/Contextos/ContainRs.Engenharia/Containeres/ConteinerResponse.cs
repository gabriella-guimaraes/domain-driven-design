namespace ContainRs.Engenharia.Containeres;

public record ConteinerResponse(string Id, string Status, string? Observacoes)
{
    public static ConteinerResponse From(Conteiner conteiner) => new(
        Id: conteiner.Id.ToString(),
        Status: conteiner.Status.ToString(),
        Observacoes: conteiner.Observacoes
    );
}
