namespace ContainRs.Api.Domain;

public class Conteiner
{
    public Guid Id { get; set; }
    /*
        ON - O contêiner está ligado e funcionando normalmente.
        OFF - O contêiner está desligado.
        STANDBY - O contêiner está em modo de espera, com consumo reduzido de energia.
        LOW_POWER - O contêiner está operando em um modo de baixa energia para economizar recursos.
        FAULT - Há uma falha no sistema de energia do contêiner.
        CHARGING - O contêiner está conectado a uma fonte de energia e sendo carregado. 
     */
    public string Status { get; set; } = "OFF";
    public string? Observacoes { get; set; }
}
