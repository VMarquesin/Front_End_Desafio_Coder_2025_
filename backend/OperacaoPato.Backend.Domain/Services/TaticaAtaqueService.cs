using System;
using System.Collections.Generic;
using System.Linq;
using OperacaoPato.Backend.Domain.Entities;
using static OperacaoPato.Backend.Domain.Services.AnalisadorVulnerabilidades;

namespace OperacaoPato.Backend.Domain.Services;

public class TaticaAtaqueService
{
    private readonly AnalisadorVulnerabilidades _analisador;
    private readonly GeradorDefesasService _geradorDefesas;
    private static readonly Random _random = new();

    public record PassoTatica(
        string Acao,
        string Descricao,
        double AltitudeAlvo,
        double VelocidadeAlvo,
        TimeSpan Duracao,
        double EnergiaNecessaria);

    public record PlanoAtaque(
        IEnumerable<PassoTatica> Sequencia,
        double ChanceSucesso,
        double EnergiaTotal,
        TimeSpan TempoTotal,
        string? DefesaRecomendada = null);

    public TaticaAtaqueService(
        AnalisadorVulnerabilidades analisador,
        GeradorDefesasService geradorDefesas)
    {
        _analisador = analisador ?? throw new ArgumentNullException(nameof(analisador));
        _geradorDefesas = geradorDefesas ?? throw new ArgumentNullException(nameof(geradorDefesas));
    }

    public PlanoAtaque GerarPlanoAtaque(PatoPrimordial pato, DroneOperacional drone)
    {
        var vulnerabilidades = _analisador.AnalisarPontosFracos(pato).ToList();
        if (!vulnerabilidades.Any())
            return GerarPlanoGenerico(pato);

        // Escolhe vulnerabilidade mais efetiva
        var vulnPrincipal = vulnerabilidades
            .OrderByDescending(v => v.ScoreEfetividade)
            .First();

        var sequencia = new List<PassoTatica>();
        var energiaTotal = 0.0;
        var tempoTotal = TimeSpan.Zero;

        // Adiciona tática de aproximação
        var aproximacao = GerarAproximacao(pato, vulnPrincipal.Tipo);
        sequencia.Add(aproximacao);
        energiaTotal += aproximacao.EnergiaNecessaria;
        tempoTotal += aproximacao.Duracao;

        // Se tem poder, gera defesa
        string? defesaRecomendada = null;
        if (pato.Poder != null)
        {
            var defesa = _geradorDefesas.GerarDefesaAleatoria(pato.Poder, vulnPrincipal.ScoreEfetividade / 100.0);
            defesaRecomendada = $"{defesa.Nome} ({defesa.Descricao})";
            
            sequencia.Add(new PassoTatica(
                "PrepararDefesa",
                $"Ativar sistema de defesa: {defesa.Nome}",
                drone.AltitudeAtual,
                0,
                TimeSpan.FromSeconds(5),
                defesa.CustoEnergia));

            energiaTotal += defesa.CustoEnergia;
            tempoTotal += TimeSpan.FromSeconds(5);
        }

        // Adiciona táticas específicas baseadas na vulnerabilidade
        var taticas = GerarTaticasEspecificas(vulnPrincipal);
        foreach (var tatica in taticas)
        {
            sequencia.Add(tatica);
            energiaTotal += tatica.EnergiaNecessaria;
            tempoTotal += tatica.Duracao;
        }

        // Calcula chance de sucesso
        var chanceBase = vulnPrincipal.ScoreEfetividade / 100.0;
        var modificadorEnergia = drone.Bateria.PorcentagemAtual / 100.0;
        var modificadorIntegridade = drone.IntegridadeFisica.PorcentagemAtual / 100.0;
        var chanceFinal = chanceBase * modificadorEnergia * modificadorIntegridade;

        return new PlanoAtaque(
            sequencia,
            Math.Round(chanceFinal * 100, 1),
            Math.Round(energiaTotal, 1),
            tempoTotal,
            defesaRecomendada);
    }

    private PassoTatica GerarAproximacao(PatoPrimordial pato, string tipoVulnerabilidade)
    {
        var alturaPatoMetros = pato.ObterAlturaEm(Enums.UnidadeComprimento.Metro).Valor;
        
        return tipoVulnerabilidade.ToLower() switch
        {
            "tamanho" => new PassoTatica(
                "AproximacaoSuperior",
                "Ganhar altitude e posicionar acima do alvo",
                alturaPatoMetros + 10,
                15,
                TimeSpan.FromSeconds(30),
                45),

            "peso" => new PassoTatica(
                "AproximacaoLateral",
                "Movimento circular mantendo distância segura",
                alturaPatoMetros,
                20,
                TimeSpan.FromSeconds(20),
                35),

            "estado" => new PassoTatica(
                "AproximacaoSilenciosa",
                "Aproximação lenta minimizando ruído",
                alturaPatoMetros - 2,
                5,
                TimeSpan.FromSeconds(45),
                25),

            _ => new PassoTatica(
                "AproximacaoPadrao",
                "Aproximação direta com velocidade moderada",
                alturaPatoMetros,
                10,
                TimeSpan.FromSeconds(25),
                30)
        };
    }

    private IEnumerable<PassoTatica> GerarTaticasEspecificas(VulnerabilidadeIdentificada vuln)
    {
        var taticas = new List<PassoTatica>();

        foreach (var taticaNome in vuln.TaticasRecomendadas)
        {
            switch (taticaNome)
            {
                case "AtaqueSuperior":
                    taticas.Add(new PassoTatica(
                        "LancamentoPedra",
                        "Soltar pedra de altitude superior",
                        30,
                        0,
                        TimeSpan.FromSeconds(10),
                        20));
                    break;

                case "CercoTatico":
                    taticas.Add(new PassoTatica(
                        "MovimentoCircular",
                        "Executar movimento circular fechando cerco",
                        15,
                        25,
                        TimeSpan.FromSeconds(30),
                        40));
                    break;

                case "AproximacaoSilenciosa":
                    taticas.Add(new PassoTatica(
                        "VooSilencioso",
                        "Reduzir velocidade e ruído",
                        8,
                        5,
                        TimeSpan.FromSeconds(45),
                        15));
                    break;

                case "AtaqueSustentado":
                    taticas.Add(new PassoTatica(
                        "DisparosContinuos",
                        "Manter pressão com disparos sucessivos",
                        12,
                        15,
                        TimeSpan.FromSeconds(20),
                        50));
                    break;

                default:
                    // Táticas genéricas para casos não mapeados
                    taticas.Add(new PassoTatica(
                        "ManobrasEvasivas",
                        "Executar manobras imprevisíveis",
                        _random.Next(10, 20),
                        _random.Next(10, 30),
                        TimeSpan.FromSeconds(_random.Next(15, 35)),
                        _random.Next(20, 40)));
                    break;
            }
        }

        return taticas;
    }

    private PlanoAtaque GerarPlanoGenerico(PatoPrimordial pato)
    {
        var alturaPatoMetros = pato.ObterAlturaEm(Enums.UnidadeComprimento.Metro).Valor;
        
        return new PlanoAtaque(
            new[]
            {
                new PassoTatica(
                    "AproximacaoCautelosa",
                    "Aproximação lenta mantendo distância segura",
                    alturaPatoMetros + 5,
                    8,
                    TimeSpan.FromSeconds(40),
                    30),
                new PassoTatica(
                    "ObservacaoTatica",
                    "Analisar padrões de movimento",
                    alturaPatoMetros + 5,
                    0,
                    TimeSpan.FromSeconds(20),
                    15),
                new PassoTatica(
                    "ManobrasEvasivas",
                    "Executar manobras imprevisíveis",
                    alturaPatoMetros,
                    15,
                    TimeSpan.FromSeconds(30),
                    35)
            },
            35.0, // chance base menor para plano genérico
            80.0, // energia total
            TimeSpan.FromSeconds(90));
    }
}