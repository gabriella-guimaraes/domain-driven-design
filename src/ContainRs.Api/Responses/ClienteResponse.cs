using ContainRs.Domain.Models;

namespace ContainRs.Api.Responses;

public record ClienteResponse(string Id, string Nome, string Email, string? Celular, IEnumerable<EnderecoResponse>? Enderecos = null)
{
    public static ClienteResponse From(Cliente cliente)
    {
        return new ClienteResponse(
            cliente.Id.ToString(),
            cliente.Nome,
            cliente.Email.Value.ToString()!,
            cliente.Celular,
            cliente.Enderecos?.Select(EnderecoResponse.From)
        );
    }
}

    public record EnderecoResponse(string Id, string? Nome, string Logradouro, string? Numero, string? Complemento, string? Bairro, string Cidade, string? Estado, string CEP)
{
    public static EnderecoResponse From(Endereco endereco)
    {
        return new EnderecoResponse(
            endereco.Id.ToString(),
            endereco.Nome,
            endereco.Rua,
            endereco.Numero,
            endereco.Complemento,
            endereco.Bairro,
            endereco.Municipio,
            endereco.Estado?.ToString(),
            endereco.CEP
        );
    }
}
public record RegistrationStatusResponse(string ClienteId, string Email, string Status)
{
    private static RegistrationStatusResponse From(Cliente cliente, string status)
    {
        return new RegistrationStatusResponse(
            cliente.Id.ToString(),
            cliente.Email.Value.ToString()!,
            status
        );
    }

    public static RegistrationStatusResponse Aprovado(Cliente cliente) =>From(cliente, "Aprovado");
    public static RegistrationStatusResponse Reprovado(Cliente cliente) => From(cliente, "Reprovado");
    public static RegistrationStatusResponse Pendente(Cliente cliente) => From(cliente, "Pendente");
}
