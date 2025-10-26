namespace OperacaoPato.Backend.Application.DTOs;

public class ResultadoOperacaoDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = "";
    public double DistanciaPercorrida { get; set; }
    public double TempoGastoSegundos { get; set; }
    public double EnergiaConsumida { get; set; }
}