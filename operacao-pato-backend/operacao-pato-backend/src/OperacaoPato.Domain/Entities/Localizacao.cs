using OperacaoPato.Domain.ValueObjects;

public class Localizacao
{
    public string Cidade { get; set; }
    public string Pais { get; set; }
    public CoordenadaGps CoordenadasGps { get; set; }
    public string PontoReferencia { get; set; }
}