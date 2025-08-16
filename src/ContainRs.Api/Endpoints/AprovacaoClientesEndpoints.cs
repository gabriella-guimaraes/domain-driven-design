using ContainRs.Api.Contracts;
using ContainRs.Api.Identity;
using ContainRs.Api.Responses;
using ContainRs.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContainRs.Api.Endpoints;

public static class AprovacaoClientesEndpoints
{
    public static IEndpointRouteBuilder MapAprovacaoClientesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup(EndpointConstants.ROUTE_CLIENTES)
            .RequireAuthorization(policy => policy.RequireRole("Suporte"))
            .WithTags(EndpointConstants.TAG_CLIENTES)
            .WithOpenApi();

        group
            .MapApproveRegistroCliente()
            .MapRejectRegistroCliente();

        return builder;
    }

    public static RouteGroupBuilder MapApproveRegistroCliente(this RouteGroupBuilder builder)
    {
        builder.MapPatch("registration/{id:guid}/approve", async (
            [FromRoute] Guid id
            , [FromServices] IRepository<Cliente> repository
            , [FromServices] UserManager<AppUser> userManager) =>
        {
            var cliente = await repository
                .GetFirstAsync(
                    c => c.Id == id,
                    c => c.Id);
            if (cliente is null) return Results.NotFound();

            var user = await userManager.FindByEmailAsync(cliente.Email.Value);
            if (user is null)
            {
                user = new AppUser
                {
                    UserName = cliente.Email.Value,
                    Email = cliente.Email.Value
                };
                await userManager.CreateAsync(user, "Alura@123");
                await userManager.AddToRoleAsync(user, "Cliente");
            }

            user.EmailConfirmed = true;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return Results.Problem(result.Errors.First().Description);

            return Results.Ok(new RegistrationStatusResponse(cliente.Id.ToString(), cliente.Email.Value, "Aprovado"));
        })
        .Produces<RegistrationStatusResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }

    public static RouteGroupBuilder MapRejectRegistroCliente(this RouteGroupBuilder builder)
    {
        builder.MapPatch("registration/{id:guid}/reject", async (
            [FromRoute] Guid id
            , [FromServices] IRepository<Cliente> repository
            , [FromServices] UserManager<AppUser> userManager) =>
        {
            var cliente = await repository
                .GetFirstAsync(
                    c => c.Id == id,
                    c => c.Id);
            if (cliente is null) return Results.NotFound();

            var user = await userManager.FindByEmailAsync(cliente.Email.Value);
            if (user is null) return Results.NotFound();

            user.EmailConfirmed = false;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return Results.Problem(result.Errors.First().Description);

            return Results.Ok(new RegistrationStatusResponse(cliente.Id.ToString(), cliente.Email.Value, "Registro não aprovado"));
        })
        .Produces<RegistrationStatusResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        return builder;
    }
}
