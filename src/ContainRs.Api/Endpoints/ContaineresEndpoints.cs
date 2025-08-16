using ContainRs.Api.Contracts;
using ContainRs.Api.Domain;
using ContainRs.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ContainRs.Api.Endpoints;

public static class ContaineresEndpoints
{
    public const string ENDPOINT_NAME_GET_CONTEINER = "GetConteiner";

    public static IEndpointRouteBuilder MapConteineresEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup(EndpointConstants.ROUTE_CONTEINERES)
            .RequireAuthorization(builder => builder.RequireRole("Cliente"))
            .WithTags(EndpointConstants.TAG_CONTEINERES)
            .WithOpenApi();

        group
            .MapGetConteinerById();

        return builder;
    }

    public static RouteGroupBuilder MapGetConteinerById(this RouteGroupBuilder builder)
    {
        builder.MapGet("{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IRepository<Conteiner> repository) =>
        {

            var conteiner = await repository
                .GetFirstAsync(
                    c => c.Id == id,
                    p => p.Id);
            if (conteiner is null) return Results.NotFound();

            return Results.Ok(ConteinerResponse.From(conteiner));
        })
        .WithName(ENDPOINT_NAME_GET_CONTEINER)
        .WithSummary("Cliente consulta informações sobre o contêiner")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ConteinerResponse>(StatusCodes.Status200OK);

        return builder;
    }
}
