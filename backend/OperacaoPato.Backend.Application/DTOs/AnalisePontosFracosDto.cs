namespace OperacaoPato.Backend.Application.DTOs;

public class AnalisePontosFracosDto
{
    public string Tipo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public double Efetividade { get; set; }
    public string[] TaticasRecomendadas { get; set; } = Array.Empty<string>();
}