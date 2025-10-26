using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Application.Services;

public class NavegacaoService
{
    public double CalcularDistancia(Coordenada origem, Coordenada destino)
    {
        var raioTerra = 6371.0; // km
        var dLat = ToRad(destino.Latitude - origem.Latitude);
        var dLon = ToRad(destino.Longitude - origem.Longitude);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(origem.Latitude)) * Math.Cos(ToRad(destino.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return raioTerra * c;
    }

    public double CalcularTempoVoo(double distancia, double velocidade)
    {
        if (velocidade <= 0)
            throw new ArgumentException("Velocidade deve ser maior que zero");
        
        return distancia / velocidade; // horas
    }

    public double CalcularConsumoEnergia(double distancia, double altitude)
    {
        var consumoBase = 0.5; // kWh por km
        var fatorAltitude = 1 + (altitude / 1000.0) * 0.1; // 10% a mais por km de altitude
        
        return distancia * consumoBase * fatorAltitude;
    }

    private static double ToRad(double graus)
    {
        return graus * Math.PI / 180.0;
    }
}