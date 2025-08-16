using ContainRs.Api.Contracts;
using ContainRs.Api.Identity;
using ContainRs.Api.Requests;
using ContainRs.Api.Responses;
using ContainRs.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace ContainRs.Api.Endpoints;

public static class ClientesEndpoints
{
    public const string ENDPOINT_NAME_GET_CLIENTE = "GetCliente";

    public static IEndpointRouteBuilder MapClientesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup(EndpointConstants.ROUTE_CLIENTES)
            .RequireAuthorization(policy => policy.RequireRole("Cliente"))
            .WithTags(EndpointConstants.TAG_CLIENTES)
            .WithOpenApi();

        group
            .MapGetClienteById()
            .MapPostClientes()
            .MapPutCliente()
            .MapDeleteCliente()
            .MapGetRegistrationStatus()
            .MapPostEndereco()
            .MapPutEndereco()
            .MapDeleteEndereco();

        return builder;
    }

    public static RouteGroupBuilder MapGetClienteById(this RouteGroupBuilder builder)
    {
        builder.MapGet("{id}", async (
            [FromRoute] Guid id
            , [FromServices] IRepository<Cliente> repository) =>
        {
            var cliente = await repository
                .GetFirstAsync(
                    c => c.Id == id, 
                    c => c.Id);
            if (cliente is null) return Results.NotFound();

            return Results.Ok(ClienteResponse.From(cliente));
        })
        .WithName(ENDPOINT_NAME_GET_CLIENTE)
        .Produces<IEnumerable<ClienteResponse>>(StatusCodes.Status200OK);
        return builder;
    }

    public static RouteGroupBuilder MapPostClientes(this RouteGroupBuilder builder)
    {
        builder.MapPost("registration", async (
            [FromBody] RegistroRequest request
            , [FromServices] IRepository<Cliente> repository) =>
        {
            var clienteExistente = await repository
                .GetFirstAsync(c => c.Email.Value == request.Email, c => c.Id);
            if (clienteExistente is not null) return Results.Conflict("Já existe cliente com o email informado!");

            var cliente = new Cliente(request.Nome, new Email(request.Email), request.CPF)
            {
                Celular = request.Celular
            };
            if (request.Endereco is not null)
            {
                cliente.AddEndereco(request.Endereco.ToModel());
            }
            await repository.AddAsync(cliente);

            return Results.CreatedAtRoute(ENDPOINT_NAME_GET_CLIENTE, new { id = cliente.Id },  ClienteResponse.From(cliente));
        })
        .AllowAnonymous()
        .Produces<ClienteResponse>(StatusCodes.Status201Created);
        return builder;
    }

    public static RouteGroupBuilder MapPutCliente(this RouteGroupBuilder builder)
    {
        builder.MapPut("{id}", async (
            [FromRoute] Guid id,
            [FromBody] RegistroRequest request,
            [FromServices] IRepository<Cliente> repository,
            CancellationToken cancellationToken) =>
        {
            var clienteExistente = await repository.GetFirstAsync(c => c.Id == id, c => c.Id);
            if (clienteExistente is null) return Results.NotFound();

            clienteExistente.Celular = request.Celular;

            await repository.UpdateAsync(clienteExistente, cancellationToken);

            return Results.Ok(ClienteResponse.From(clienteExistente));
        })
        .Produces<ClienteResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }

    public static RouteGroupBuilder MapDeleteCliente(this RouteGroupBuilder builder)
    {
        builder.MapDelete("{id}", async (
            [FromRoute] Guid id
            , [FromServices] IRepository<Cliente> repository
            , [FromServices] UserManager<AppUser> userManager
            , CancellationToken cancellationToken) =>
        {
            var clienteExistente = await repository.GetFirstAsync(c => c.Id == id, c => c.Id);
            if (clienteExistente is null) return Results.NotFound();

            var user = await userManager.FindByEmailAsync(clienteExistente.Email.Value);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (user is not null) await userManager.DeleteAsync(user);
            await repository.RemoveAsync(clienteExistente, cancellationToken);

            scope.Complete();
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }

    public static RouteGroupBuilder MapGetRegistrationStatus(this RouteGroupBuilder builder)
    {
        builder.MapGet("registration/status", 
            async (
                [FromQuery] string email
                , [FromServices] IRepository<Cliente> repository
                , [FromServices] UserManager<AppUser> userManager) =>
            {
                var cliente = await repository
                    .GetFirstAsync(c => c.Email.Value.Equals(email), c => c.Id);
                if (cliente is null) return Results.NotFound();

                // verificar se já existe user associado ao email do cliente
                var user = await userManager.FindByEmailAsync(cliente.Email.Value);

                // se não houver user, retornar status Pendente
                if (user is null) return Results.Ok(RegistrationStatusResponse.Pendente(cliente));

                // se houver user e EmailConfirmed for false, retornar Em análise
                if (!user.EmailConfirmed) return Results.Ok(RegistrationStatusResponse.Reprovado(cliente));

                // se houver user e EmailConfirmed for true, retornar Aprovado
                return Results.Ok(RegistrationStatusResponse.Aprovado(cliente));
            })
            .AllowAnonymous()
            .Produces<RegistrationStatusResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        return builder;
    }

    public static RouteGroupBuilder MapPostEndereco(this RouteGroupBuilder builder)
    {
        builder.MapPost("{id}/enderecos", async (
            [FromRoute] Guid id,
            [FromBody] EnderecoRequest request,
            [FromServices] IRepository<Cliente> repository,
            CancellationToken cancellationToken) =>
        {
            var cliente = await repository.GetFirstAsync(c => c.Id == id, c => c.Id);
            if (cliente is null) return Results.NotFound();

            cliente.AddEndereco(request.ToModel());
            await repository.UpdateAsync(cliente, cancellationToken);

            return Results.CreatedAtRoute(ENDPOINT_NAME_GET_CLIENTE, new { id = cliente.Id }, ClienteResponse.From(cliente));
        })
        .Produces<ClienteResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }

    public static RouteGroupBuilder MapPutEndereco(this RouteGroupBuilder builder)
    {
        builder.MapPut("{id:guid}/enderecos/{idEndereco:guid}", async (
            [FromRoute] Guid id,
            [FromRoute] Guid idEndereco,
            [FromBody] EnderecoRequest request,
            [FromServices] IRepository<Cliente> repository,
            CancellationToken cancellationToken) =>
        {
            var cliente = await repository.GetFirstAsync(c => c.Id == id, c => c.Id);
            if (cliente is null) return Results.NotFound();

            var endereco = cliente.Enderecos.FirstOrDefault(e => e.Id == idEndereco);
            if (endereco is null) return Results.NotFound();

            endereco.CEP = request.CEP ?? endereco.CEP;
            endereco.Rua = request.Rua ?? endereco.Rua;
            endereco.Numero = request.Numero ?? endereco.Numero;
            endereco.Complemento = request.Complemento ?? endereco.Complemento;
            endereco.Bairro = request.Bairro ?? endereco.Bairro;
            endereco.Municipio = request.Municipio ?? endereco.Municipio;
            if (request.Estado is not null)
                endereco.Estado = UfStringConverter.From(request.Estado);

            await repository.UpdateAsync(cliente, cancellationToken);

            return Results.Ok(ClienteResponse.From(cliente));
        })
        .Produces<ClienteResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }

    public static RouteGroupBuilder MapDeleteEndereco(this RouteGroupBuilder builder)
    {
        builder.MapDelete("{id:guid}/enderecos/{idEndereco:guid}", async (
            [FromRoute] Guid id,
            [FromRoute] Guid idEndereco,
            [FromServices] IRepository<Cliente> repository,
            CancellationToken cancellationToken) =>
        {
            var cliente = await repository.GetFirstAsync(c => c.Id == id, c => c.Id);
            if (cliente is null) return Results.NotFound();

            var endereco = cliente.Enderecos.FirstOrDefault(e => e.Id == idEndereco);
            if (endereco is null) return Results.NotFound();

            cliente.RemoveEndereco(endereco);
            await repository.UpdateAsync(cliente, cancellationToken);

            return Results.Ok(ClienteResponse.From(cliente));
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }
}
