namespace ContainRs.Vendas.Propostas
{
    public class Endereco
    {
        public required string CEP { get; set; }
        public string? Referencias { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
