using System;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Domain.Entities;

public class DroneOperacional : Drone
{
    public NivelRecurso Bateria { get; private set; }
    public NivelRecurso Combustivel { get; private set; }
    public NivelRecurso IntegridadeFisica { get; private set; }
    public Coordenada Posicao { get; private set; }
    public double VelocidadeAtual { get; private set; }
    public double AltitudeAtual { get; private set; }
    public DateTime UltimaAtualizacao { get; private set; }

    public DroneOperacional(
        string numeroSerie,
        string marca,
        string fabricante,
        string paisOrigem,
        NivelRecurso bateria,
        NivelRecurso combustivel,
        NivelRecurso integridadeFisica,
        Coordenada posicaoInicial) 
        : base(numeroSerie, marca, fabricante, paisOrigem)
    {
        Bateria = bateria ?? throw new ArgumentNullException(nameof(bateria));
        Combustivel = combustivel ?? throw new ArgumentNullException(nameof(combustivel));
        IntegridadeFisica = integridadeFisica ?? throw new ArgumentNullException(nameof(integridadeFisica));
        Posicao = posicaoInicial ?? throw new ArgumentNullException(nameof(posicaoInicial));
        VelocidadeAtual = 0;
        AltitudeAtual = 0;
        UltimaAtualizacao = DateTime.UtcNow;
    }

    public void AtualizarPosicao(Coordenada novaPosicao, double novaVelocidade, double novaAltitude, DateTime timestamp)
    {
        if (novaPosicao == null)
            throw new ArgumentNullException(nameof(novaPosicao));
        if (novaVelocidade < 0)
            throw new ArgumentException("Velocidade deve ser positiva", nameof(novaVelocidade));
        if (novaAltitude < 0)
            throw new ArgumentException("Altitude deve ser positiva", nameof(novaAltitude));
        if (timestamp <= UltimaAtualizacao)
            throw new ArgumentException("Timestamp deve ser mais recente que última atualização", nameof(timestamp));

        // Cálculo de consumo baseado no tempo e movimento
        var tempoDecorrido = (timestamp - UltimaAtualizacao).TotalHours;
        var distanciaPercorrida = CalcularDistancia(Posicao, novaPosicao);
        
        // Consumo de combustível baseado em distância e altitude
        var consumoCombustivel = CalcularConsumoCombustivel(distanciaPercorrida, novaAltitude, tempoDecorrido);
        Combustivel.Consumir(consumoCombustivel);

        // Consumo de bateria (constante + baseado em operação)
        var consumoBateria = CalcularConsumoBateria(tempoDecorrido, novaVelocidade > 0);
        Bateria.Consumir(consumoBateria);

        // Atualiza estado
        Posicao = novaPosicao;
        VelocidadeAtual = novaVelocidade;
        AltitudeAtual = novaAltitude;
        UltimaAtualizacao = timestamp;
    }

    public void RegistrarDano(double percentualDano)
    {
        if (percentualDano <= 0 || percentualDano > 100)
            throw new ArgumentException("Percentual de dano deve estar entre 0 e 100", nameof(percentualDano));

        var danoAbsoluto = (IntegridadeFisica.ValorMaximo * percentualDano) / 100;
        IntegridadeFisica.Consumir(danoAbsoluto);
    }

    public bool PodeOperar()
    {
        return !Bateria.EmNivelCritico &&
               !Combustivel.EmNivelCritico &&
               IntegridadeFisica.PorcentagemAtual > 30;
    }

    private double CalcularDistancia(Coordenada origem, Coordenada destino)
    {
        const double R = 6371.0; // Raio da Terra em km
        var dLat = ToRad(destino.Latitude - origem.Latitude);
        var dLon = ToRad(destino.Longitude - origem.Longitude);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(origem.Latitude)) * Math.Cos(ToRad(destino.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private double CalcularConsumoCombustivel(double distanciaKm, double altitudeMetros, double horasVoo)
    {
        // Consumo base por hora de voo
        const double ConsumoBasePorHora = 2.5;
        
        // Adicional por km percorrido
        const double ConsumoPorKm = 0.5;
        
        // Adicional por altitude (cresce exponencialmente)
        var consumoAltitude = Math.Pow(altitudeMetros / 1000.0, 1.5) * 1.2;

        return (ConsumoBasePorHora * horasVoo) + 
               (ConsumoPorKm * distanciaKm) + 
               (consumoAltitude * horasVoo);
    }

    private double CalcularConsumoBateria(double horasVoo, bool emMovimento)
    {
        // Consumo base por hora (sistemas essenciais)
        const double ConsumoBasePorHora = 1.0;
        
        // Adicional quando em movimento (sensores, cálculos)
        const double ConsumoAdicionalMovimento = 0.5;

        return (ConsumoBasePorHora * horasVoo) + 
               (emMovimento ? ConsumoAdicionalMovimento * horasVoo : 0);
    }

    private static double ToRad(double deg) => deg * Math.PI / 180.0;

    private DroneOperacional() : base("TEMP_SERIAL", "TEMP_MARCA", "TEMP_FAB", "TEMP_PAIS")
    { 
        Bateria = new NivelRecurso(0, 100, "kWh");
        Combustivel = new NivelRecurso(0, 100, "L");
        IntegridadeFisica = new NivelRecurso(0, 100, "%");
        Posicao = new Coordenada(0, 0);
        VelocidadeAtual = 0;
        AltitudeAtual = 0;
        UltimaAtualizacao = DateTime.UtcNow;
    } // EF
}