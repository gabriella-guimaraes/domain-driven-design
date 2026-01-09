using ContainRs.Vendas.Locacoes;
using System.Transactions;

namespace ContainRs.Vendas.Propostas
{
    public class ApovarProposta
    {
        public ApovarProposta(Guid idPedido, Guid idProposta)
        {
            IdPedido = idPedido;
            IdProposta = idProposta;
        }

        public Guid IdPedido { get; }
        public Guid IdProposta { get; }

    }
}
