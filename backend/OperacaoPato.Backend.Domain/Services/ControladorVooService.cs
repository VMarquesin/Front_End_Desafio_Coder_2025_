using System;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Domain.Services;

public class ControladorVooService
{
    public record InstrucaoVoo(
        Coordenada Destino,
        double VelocidadeAlvo,
        double AltitudeAlvo,
        TimeSpan TempoEstimado);

    public class ResultadoExecucao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public double DistanciaPercorrida { get; set; }
        public TimeSpan TempoGasto { get; set; }
        public double EnergiaConsumida { get; set; }
    }

    private const double VelocidadeMaxima = 50.0; // m/s
    private const double AltitudeMaxima = 100.0; // metros
    private const double TaxaSubidaMaxima = 5.0; // m/s
    private const double AceleracaoMaxima = 2.0; // m/s²

    public InstrucaoVoo CalcularInstrucaoVoo(
        Coordenada origem,
        Coordenada destino,
        double velocidadeAtual,
        double altitudeAtual,
        double altitudeAlvo)
    {
        // Validações
        if (altitudeAlvo < 0 || altitudeAlvo > AltitudeMaxima)
            throw new ArgumentException($"Altitude alvo deve estar entre 0 e {AltitudeMaxima}m");

        // Calcula distância
        var distanciaKm = CalcularDistancia(origem, destino);
        var distanciaMetros = distanciaKm * 1000;

        // Calcula diferença de altitude
        var deltaAltitude = altitudeAlvo - altitudeAtual;
        var tempoSubida = Math.Abs(deltaAltitude) / TaxaSubidaMaxima;

        // Calcula velocidade ideal
        var velocidadeIdeal = CalcularVelocidadeIdeal(distanciaMetros, deltaAltitude);
        var tempoAceleracao = Math.Abs(velocidadeIdeal - velocidadeAtual) / AceleracaoMaxima;

        // Calcula tempo total estimado
        var tempoVooHorizontal = distanciaMetros / velocidadeIdeal;
        var tempoTotal = Math.Max(tempoVooHorizontal, tempoSubida) + tempoAceleracao;

        return new InstrucaoVoo(
            destino,
            velocidadeIdeal,
            altitudeAlvo,
            TimeSpan.FromSeconds(tempoTotal));
    }

    public ResultadoExecucao ExecutarInstrucao(
        Entities.DroneOperacional drone,
        InstrucaoVoo instrucao)
    {
        try
        {
            // Valida recursos
            if (!drone.PodeOperar())
                return new ResultadoExecucao
                {
                    Sucesso = false,
                    Mensagem = "Drone não tem recursos suficientes para operar"
                };

            var inicio = DateTime.UtcNow;
            
            // Atualiza posição do drone
            drone.AtualizarPosicao(
                instrucao.Destino,
                instrucao.VelocidadeAlvo,
                instrucao.AltitudeAlvo,
                inicio.Add(instrucao.TempoEstimado));

            // Calcula métricas
            var distancia = CalcularDistancia(drone.Posicao, instrucao.Destino) * 1000; // em metros
            var energiaGasta = CalcularEnergiaGasta(
                distancia,
                Math.Abs(instrucao.AltitudeAlvo - drone.AltitudeAtual),
                instrucao.TempoEstimado.TotalSeconds,
                instrucao.VelocidadeAlvo);

            return new ResultadoExecucao
            {
                Sucesso = true,
                Mensagem = "Instrução executada com sucesso",
                DistanciaPercorrida = Math.Round(distancia, 2),
                TempoGasto = instrucao.TempoEstimado,
                EnergiaConsumida = Math.Round(energiaGasta, 2)
            };
        }
        catch (Exception ex)
        {
            return new ResultadoExecucao
            {
                Sucesso = false,
                Mensagem = $"Erro ao executar instrução: {ex.Message}"
            };
        }
    }

    private double CalcularVelocidadeIdeal(double distanciaMetros, double deltaAltitude)
    {
        // Para movimentos verticais significativos, reduz velocidade
        var fatorAltitude = Math.Abs(deltaAltitude) > 10 ? 0.7 : 1.0;
        
        // Velocidade base proporcional à distância
        var velocidadeBase = Math.Min(VelocidadeMaxima,
            Math.Max(5.0, distanciaMetros / 100.0));

        return velocidadeBase * fatorAltitude;
    }

    private static double CalcularDistancia(Coordenada origem, Coordenada destino)
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

    private static double CalcularEnergiaGasta(
        double distanciaMetros,
        double deltaAltitude,
        double tempoSegundos,
        double velocidade)
    {
        // Energia base por tempo de operação
        var energiaBase = tempoSegundos * 0.5;

        // Energia para movimento horizontal
        var energiaMovimento = (distanciaMetros / 1000.0) * velocidade * 0.3;

        // Energia para movimento vertical
        var energiaAltitude = Math.Pow(deltaAltitude / 10.0, 1.5) * 2.0;

        return energiaBase + energiaMovimento + energiaAltitude;
    }

    private static double ToRad(double deg) => deg * Math.PI / 180.0;
}