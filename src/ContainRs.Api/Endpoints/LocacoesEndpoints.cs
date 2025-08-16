using ContainRs.Api.Contracts;
using ContainRs.Api.Domain;
using ContainRs.Api.Extensions;
using ContainRs.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ContainRs.Api.Endpoints;

public static class LocacoesEndpoints
{
    public static IEndpointRouteBuilder MapLocacoesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup(EndpointConstants.ROUTE_LOCACOES)
            .RequireAuthorization(builder => builder.RequireRole("Cliente"))
            .WithTags(EndpointConstants.TAG_LOCACAO)
            .WithOpenApi();

        group
            .MapGetLocacoes();

        return builder;
    }

    public static RouteGroupBuilder MapGetLocacoes(this RouteGroupBuilder builder)
    {
        builder.MapGet("", async (
            HttpContext context,
            [FromServices] IRepository<Locacao> repository) =>
        {
            var clienteId = context.GetClienteId();
            if (clienteId is null) return Results.Unauthorized();

            var locacoes = await repository
                .GetWhereAsync(l => l.ClienteId == clienteId.Value);
            return Results.Ok(locacoes.Select(LocacaoResponse.From));
        })
        .WithSummary("Lista o histórico de locações do cliente");

        return builder;
    }
}
