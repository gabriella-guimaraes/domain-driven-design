namespace ContainRs.Api.Extensions;

/// <summary>
/// Método de Extenção para o HttpContext
/// </summary>
public static class HttpContextExtensions
{
    public static Guid? GetClienteId(this HttpContext context)
    {
        var clienteId = context.User.Claims
                .Where(c => c.Type.Equals("ClienteId"))
                .Select(c => c.Value)
                .FirstOrDefault();

        return clienteId is not null
            ? Guid.Parse(clienteId)
            : null;
    }
}
