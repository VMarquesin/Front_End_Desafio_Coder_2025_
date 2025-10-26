namespace OperacaoPato.Backend.Application.DTOs;

public class InstrucaoVooDto
{
    public double LatitudeDestino { get; set; }
    public double LongitudeDestino { get; set; }
    public double VelocidadeAlvo { get; set; }
    public double AltitudeAlvo { get; set; }
}