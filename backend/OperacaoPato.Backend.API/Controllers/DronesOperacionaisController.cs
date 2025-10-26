using Microsoft.AspNetCore.Mvc;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Services;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DronesOperacionaisController : ControllerBase
{
    private readonly ControladorVooService _controladorVoo;
    private readonly AnalisadorVulnerabilidades _analisador;
    private static readonly Dictionary<string, DroneOperacional> _dronesAtivos = new();

    public DronesOperacionaisController(
        ControladorVooService controladorVoo,
        AnalisadorVulnerabilidades analisador)
    {
        _controladorVoo = controladorVoo;
        _analisador = analisador;
    }

    [HttpPost("inicializar")]
    public ActionResult<DroneOperacionalStatusDto> InicializarDrone(
        [FromBody] string numeroSerie)
    {
        if (_dronesAtivos.ContainsKey(numeroSerie))
            return Conflict($"Drone {numeroSerie} já está ativo");

        var drone = new DroneOperacional(
            numeroSerie: numeroSerie,
            marca: "CaçaPatos",
            fabricante: "DSIN",
            paisOrigem: "Brasil",
            bateria: new NivelRecurso(100, 100, "kWh"),
            combustivel: new NivelRecurso(50, 50, "L"),
            integridadeFisica: new NivelRecurso(100, 100, "%"),
            posicaoInicial: new Coordenada(-23.5505, -46.6333));

        _dronesAtivos[numeroSerie] = drone;

        return Ok(MapearStatus(drone));
    }

    [HttpGet("{numeroSerie}/status")]
    public ActionResult<DroneOperacionalStatusDto> ObterStatus(string numeroSerie)
    {
        if (!_dronesAtivos.TryGetValue(numeroSerie, out var drone))
            return NotFound($"Drone {numeroSerie} não está ativo");

        return Ok(MapearStatus(drone));
    }

    [HttpPost("{numeroSerie}/mover")]
    public ActionResult<ResultadoOperacaoDto> ExecutarMovimento(
        string numeroSerie,
        [FromBody] InstrucaoVooDto instrucao)
    {
        if (!_dronesAtivos.TryGetValue(numeroSerie, out var drone))
            return NotFound($"Drone {numeroSerie} não está ativo");

        try
        {
            var destino = new Coordenada(instrucao.LatitudeDestino, instrucao.LongitudeDestino);
            var comandoVoo = _controladorVoo.CalcularInstrucaoVoo(
                drone.Posicao,
                destino,
                drone.VelocidadeAtual,
                drone.AltitudeAtual,
                instrucao.AltitudeAlvo);

            var resultado = _controladorVoo.ExecutarInstrucao(drone, comandoVoo);

            return Ok(new ResultadoOperacaoDto
            {
                Sucesso = resultado.Sucesso,
                Mensagem = resultado.Mensagem,
                DistanciaPercorrida = resultado.DistanciaPercorrida,
                TempoGastoSegundos = resultado.TempoGasto.TotalSeconds,
                EnergiaConsumida = resultado.EnergiaConsumida
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultadoOperacaoDto
            {
                Sucesso = false,
                Mensagem = $"Erro ao executar movimento: {ex.Message}"
            });
        }
    }

    [HttpPost("{numeroSerie}/registrar-dano")]
    public ActionResult<DroneOperacionalStatusDto> RegistrarDano(
        string numeroSerie,
        [FromBody] double percentualDano)
    {
        if (!_dronesAtivos.TryGetValue(numeroSerie, out var drone))
            return NotFound($"Drone {numeroSerie} não está ativo");

        try
        {
            drone.RegistrarDano(percentualDano);
            return Ok(MapearStatus(drone));
        }
        catch (Exception ex)
        {
            return BadRequest(new ResultadoOperacaoDto
            {
                Sucesso = false,
                Mensagem = $"Erro ao registrar dano: {ex.Message}"
            });
        }
    }

    [HttpPost("{numeroSerie}/analisar-pato/{patoId}")]
    public ActionResult<IEnumerable<AnalisePontosFracosDto>> AnalisarPato(
        string numeroSerie,
        Guid patoId,
        [FromServices] IPatoRepository patoRepository)
    {
        if (!_dronesAtivos.TryGetValue(numeroSerie, out _))
            return NotFound($"Drone {numeroSerie} não está ativo");

        var pato = patoRepository.ObterPorIdAsync(patoId).Result;
        if (pato == null)
            return NotFound($"Pato {patoId} não encontrado");

        var analise = _analisador.AnalisarPontosFracos(pato);
        
        return Ok(analise.Select(v => new AnalisePontosFracosDto
        {
            Tipo = v.Tipo,
            Descricao = v.Descricao,
            Efetividade = v.ScoreEfetividade,
            TaticasRecomendadas = v.TaticasRecomendadas.ToArray()
        }));
    }

    private static DroneOperacionalStatusDto MapearStatus(DroneOperacional drone)
    {
        return new DroneOperacionalStatusDto
        {
            NumeroSerie = drone.NumeroSerie,
            BateriaPorcentagem = drone.Bateria.PorcentagemAtual,
            CombustivelPorcentagem = drone.Combustivel.PorcentagemAtual,
            IntegridadePorcentagem = drone.IntegridadeFisica.PorcentagemAtual,
            Latitude = drone.Posicao.Latitude,
            Longitude = drone.Posicao.Longitude,
            VelocidadeAtual = drone.VelocidadeAtual,
            AltitudeAtual = drone.AltitudeAtual,
            PodeOperar = drone.PodeOperar()
        };
    }
}