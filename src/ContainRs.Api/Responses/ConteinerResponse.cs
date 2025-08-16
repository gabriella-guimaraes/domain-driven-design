using ContainRs.Api.Domain;

namespace ContainRs.Api.Responses;

public record ConteinerResponse(string Id, string Status, string? Observacoes)
{
    public static ConteinerResponse From(Conteiner conteiner) => new(
        Id: conteiner.Id.ToString(),
        Status: conteiner.Status.ToString(),
        Observacoes: conteiner.Observacoes
    );
}
