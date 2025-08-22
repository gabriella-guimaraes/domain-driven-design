namespace ContainRs.Clientes.Cadastro
{
    public interface IAcessoManager
    {
        /// <summary>
        /// Adiciona um cliente ao sistema.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AdicionarClienteAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Remover um cliente do sistema.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoverClienteAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Verifica se o cliente possui acesso ao sistema.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool?> ClientePossuiAcesso(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Bloqueia um cliente no sistema, impedindo o acesso.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BloquearClienteAsync (string email, CancellationToken cancellationToken);
    }
}
