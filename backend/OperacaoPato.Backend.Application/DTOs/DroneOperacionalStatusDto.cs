namespace OperacaoPato.Backend.Application.DTOs;

public class DroneOperacionalStatusDto
{
    public string NumeroSerie { get; set; } = "";
    public double BateriaPorcentagem { get; set; }
    public double CombustivelPorcentagem { get; set; }
    public double IntegridadePorcentagem { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double VelocidadeAtual { get; set; }
    public double AltitudeAtual { get; set; }
    public bool PodeOperar { get; set; }
}