using ContainRs.Api.Contracts;
using ContainRs.Api.Domain;
using ContainRs.Api.Requests;
using ContainRs.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace ContainRs.Api.Endpoints;

public static class PropostasEndpoints
{
    public const string ENDPOINT_NAME_GET_PROPOSTA = "GetProposta";

    public static IEndpointRouteBuilder MapPropostasEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup(EndpointConstants.ROUTE_SOLICITACOES)
            .WithTags(EndpointConstants.TAG_LOCACAO)
            .WithOpenApi();

        group
            .MapPostProposta()
            .MapGetPropostas()
            .MapGetPropostaById()
            .MapPatchAcceptProposta()
            .MapPatchRejectProposta()
            .MapPostComentarioProposta();

        return builder;
    }

    public static RouteGroupBuilder MapPostProposta(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("{id:guid}/proposals", async(
                [FromRoute] Guid id,
                [FromForm] PropostaRequest request,
                [FromServices] IRepository <Solicitacao> repoSolicitacao,
                [FromServices] IRepository<Proposta> repoProposta
                ) =>
            {
                // salvar arquivo no sistema de arquivos configurado (não será feito neste curso)

                var solicitacao = await repoSolicitacao
                    .GetFirstAsync(s => s.Id == id, s => s.Id);
                if (solicitacao is null) return Results.NotFound();

                var proposta = new Proposta()
                {
                    Id = Guid.NewGuid(),
                    ValorTotal = request.ValorTotal,
                    DataCriacao = DateTime.Now,
                    DataExpiracao = request.DataExpiracao,
                    NomeArquivo = request.Arquivo.FileName,
                    SolicitacaoId = solicitacao.Id
                };

                await repoProposta.AddAsync(proposta);

                return Results.CreatedAtRoute(
                    ENDPOINT_NAME_GET_PROPOSTA, 
                    new { proposta.Id }, 
                    PropostaResponse.From(proposta));
            })
            // deveria ser Comercial, mas para não criarmos usuários com papéis diferentes, usaremos o papel (role) Suporte
            .RequireAuthorization(policy => policy.RequireRole("Suporte"))
            .WithSummary("Vendedor envia proposta de locação")
            .Produces(StatusCodes.Status404NotFound)
            .Produces<PropostaResponse>(StatusCodes.Status201Created);
        return builder;
    }

    public static RouteGroupBuilder MapGetPropostaById(this RouteGroupBuilder builder)
    {
        builder.MapGet("{id:guid}/proposals/{propostaId:guid}", async (
            [FromRoute] Guid id,
            [FromRoute] Guid propostaId,
            [FromServices] IRepository<Proposta> repository) =>
        {

            var proposta = await repository
                .GetFirstAsync(
                    p => p.Id == propostaId && p.SolicitacaoId == id,
                    p => p.Id);
            if (proposta is null) return Results.NotFound();

            return Results.Ok(PropostaResponse.From(proposta));
        })
        .WithName(ENDPOINT_NAME_GET_PROPOSTA)
        .RequireAuthorization(policy => policy.RequireRole("Cliente"))
        .WithSummary("Cliente consulta detalhes de uma proposta de locação")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<PropostaResponse>(StatusCodes.Status200OK);

        return builder;
    }

    public static RouteGroupBuilder MapGetPropostas(this RouteGroupBuilder builder)
    {
        builder.MapGet("{id:guid}/proposals", async (
            [FromRoute] Guid id,
            [FromServices] IRepository<Solicitacao> repository) =>
        {

            var solicitacao = await repository
                .GetFirstAsync(
                    s => s.Id == id,
                    s => s.Id);
            if (solicitacao is null) return Results.NotFound();

        return Results.Ok(solicitacao.Propostas.Select(p => PropostaResponse.From(p)));
        })
        .RequireAuthorization(policy => policy.RequireRole("Cliente"))
        .WithSummary("Cliente consulta as propostas para uma solicitação de locação")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<IEnumerable<PropostaResponse>>(StatusCodes.Status200OK);

        return builder;
    }

    public static RouteGroupBuilder MapPatchAcceptProposta(this RouteGroupBuilder builder)
    {
        builder.MapPatch("{id:guid}/proposals/{propostaId:guid}/accept", async (
            [FromRoute] Guid id,
            [FromRoute] Guid propostaId,
            [FromServices] IRepository<Proposta> repoProposta,
            [FromServices] IRepository<Locacao> repoLocacao) =>
        {

            var proposta = await repoProposta
                .GetFirstAsync(
                    p => p.Id == propostaId && p.SolicitacaoId == id,
                    p => p.Id);
            if (proposta is null) return Results.NotFound();

            proposta.Status = StatusProposta.Aceita;

            // criar locação a partir da proposta aceita
            var locacao = new Locacao()
            {
                PropostaId = proposta.Id,
                DataInicio = DateTime.Now,
                DataPrevistaEntrega = proposta.Solicitacao.DataInicioOperacao.AddDays(-proposta.Solicitacao.DisponibilidadePrevia),
                DataTermino = proposta.Solicitacao.DataInicioOperacao.AddDays(proposta.Solicitacao.DuracaoPrevistaLocacao)
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await repoProposta.UpdateAsync(proposta);
            await repoLocacao.AddAsync(locacao);

            scope.Complete();

            return Results.Ok(PropostaResponse.From(proposta));
        })
        .WithSummary("Cliente aceita proposta de locação.")
        .RequireAuthorization(policy => policy.RequireRole("Cliente"))
        .Produces(StatusCodes.Status404NotFound)
        .Produces<PropostaResponse>(StatusCodes.Status200OK);

        return builder;
    }

    public static RouteGroupBuilder MapPatchRejectProposta(this RouteGroupBuilder builder)
    {
        builder.MapPatch("{id:guid}/proposals/{propostaId:guid}/reject", async (
            [FromRoute] Guid id,
            [FromRoute] Guid propostaId,
            [FromServices] IRepository<Proposta> repository) =>
        {

            var proposta = await repository
                .GetFirstAsync(
                    p => p.Id == propostaId && p.SolicitacaoId == id,
                    p => p.Id);
            if (proposta is null) return Results.NotFound();

            proposta.Status = StatusProposta.Recusada;
            await repository.UpdateAsync(proposta);

            return Results.Ok(PropostaResponse.From(proposta));
        })
        .WithSummary("Cliente rejeita proposta de locação.")
        .RequireAuthorization(policy => policy.RequireRole("Cliente"))
        .Produces(StatusCodes.Status404NotFound)
        .Produces<PropostaResponse>(StatusCodes.Status200OK);

        return builder;
    }

    public static RouteGroupBuilder MapPostComentarioProposta(this RouteGroupBuilder builder)
    {
        builder.MapPost("{id:guid}/proposals/{propostaId:guid}/comment", async (
            [FromRoute] Guid id,
            [FromRoute] Guid propostaId,
            [FromBody] ComentarioRequest request,
            HttpContext context,
            [FromServices] IRepository<Proposta> repository) =>
        {

            var proposta = await repository
                .GetFirstAsync(
                    p => p.Id == propostaId && p.SolicitacaoId == id,
                    p => p.Id);
            if (proposta is null) return Results.NotFound();

            string? quem = context.User.Identity?.Name;
            if (quem is null) return Results.Unauthorized();

            proposta.AddComentario(new Comentario()
            {
                Id = Guid.NewGuid(),
                Data = DateTime.Now,
                Usuario = quem,
                Texto = request.Comentario
            });

            await repository.UpdateAsync(proposta);

            return Results.Ok(PropostaResponse.From(proposta));
        })
        .WithSummary("Vendedor/Cliente comenta proposta.")
        .RequireAuthorization(policy => policy.RequireRole("Cliente", "Suporte"))
        .Produces(StatusCodes.Status404NotFound)
        .Produces<PropostaResponse>(StatusCodes.Status200OK);

        return builder;
    }
}
