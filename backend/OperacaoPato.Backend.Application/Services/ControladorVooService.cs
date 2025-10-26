using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Application.Services;

public class ControladorVooService
{
    private readonly NavegacaoService _navegacao;
    private readonly MonitorStatusService _monitor;

    public ControladorVooService(
        NavegacaoService navegacao,
        MonitorStatusService monitor)
    {
        _navegacao = navegacao;
        _monitor = monitor;
    }

    public InstrucaoVoo CalcularInstrucaoVoo(
        DroneOperacional drone, 
        Coordenada destino, 
        double altitudeAlvo)
    {
        if (!_monitor.PodeOperar(drone.Bateria, drone.Combustivel, drone.IntegridadeFisica))
            throw new InvalidOperationException("Drone não está em condições de operar");

        var distancia = _navegacao.CalcularDistancia(drone.Posicao, destino);
        var estimativaEnergia = _navegacao.CalcularConsumoEnergia(distancia, altitudeAlvo);

        if (estimativaEnergia > drone.Bateria.Valor)
            throw new InvalidOperationException("Energia insuficiente para completar o trajeto");

        return new InstrucaoVoo
        {
            CoordenadaDestino = destino,
            AltitudeAlvo = altitudeAlvo,
            ConsumoEnergiaEstimado = estimativaEnergia,
            DistanciaEstimada = distancia,
            TempoEstimado = _navegacao.CalcularTempoVoo(distancia, drone.VelocidadeAtual)
        };
    }

    public ResultadoOperacao ExecutarInstrucao(DroneOperacional drone, InstrucaoVoo instrucao)
    {
        try
        {
            // Atualiza posição do drone
            drone.AtualizarPosicao(
                instrucao.CoordenadaDestino,
                drone.VelocidadeAtual,
                instrucao.AltitudeAlvo,
                DateTime.UtcNow);

            return new ResultadoOperacao
            {
                Sucesso = true,
                Mensagem = "Movimento concluído com sucesso",
                DistanciaPercorrida = instrucao.DistanciaEstimada,
                EnergiaConsumida = instrucao.ConsumoEnergiaEstimado,
                TempoGasto = TimeSpan.FromHours(instrucao.TempoEstimado)
            };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacao
            {
                Sucesso = false,
                Mensagem = $"Erro ao executar movimento: {ex.Message}"
            };
        }
    }
}

public class InstrucaoVoo
{
    public Coordenada CoordenadaDestino { get; set; } = new(0, 0);
    public double AltitudeAlvo { get; set; }
    public double ConsumoEnergiaEstimado { get; set; }
    public double DistanciaEstimada { get; set; }
    public double TempoEstimado { get; set; }
}

public class ResultadoOperacao
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = "";
    public double DistanciaPercorrida { get; set; }
    public TimeSpan TempoGasto { get; set; }
    public double EnergiaConsumida { get; set; }
}