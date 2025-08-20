using ContainRs.Api.Domain;

namespace ContainRs.Api.Locacoes;

public record SolicitacaoResponse(string Id, string Status, string Finalidade)
{
    public static SolicitacaoResponse From(PedidoLocacao solicitacao) => new(
        Id: solicitacao.Id.ToString(),
        Status: solicitacao.Status.ToString(),
        Finalidade: solicitacao.Finalidade
    );
}

public record PropostaResponse(string Id, string Status, decimal Valor, DateTime DataExpiracao)
{
    public static PropostaResponse From(Proposta proposta) => new(
        Id: proposta.Id.ToString(),
        Status: proposta.Situacao.ToString(),
        Valor: proposta.ValorTotal,
        DataExpiracao: proposta.DataExpiracao
    );
}

public record LocacaoResponse(string Id, string Status, DateTime DataInicio, DateTime DataTermino, DateTime DataPrevistaEntrega)
{
    public static LocacaoResponse From(Locacao locacao) => new(
        Id: locacao.Id.ToString(),
        Status: locacao.Status.ToString(),
        DataInicio: locacao.DataInicio,
        DataTermino: locacao.DataTermino,
        DataPrevistaEntrega: locacao.DataPrevistaEntrega
    );
}
