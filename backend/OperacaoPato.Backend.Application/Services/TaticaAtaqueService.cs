using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Application.Services;

public class TaticaAtaqueService
{
    private readonly Dictionary<string, (double Peso, string Descricao)> _regrasAtaque = new()
    {
        { "AtaqueFrontal", (0.7, "Ataque direto com força máxima") },
        { "AtaqueFlancos", (0.8, "Ataque pelos lados para confundir") },
        { "AtaqueSurpresa", (0.9, "Aproximação furtiva e ataque rápido") },
        { "AtaqueConjunto", (1.0, "Coordenação com outros drones") }
    };

    public EstrategiaAtaque CalcularMelhorEstrategia(
        PatoPrimordial pato,
        double distancia,
        double altitude,
        IEnumerable<string> vulnerabilidadesIdentificadas)
    {
        var estrategias = _regrasAtaque.Keys.ToList();
        var melhorScore = 0.0;
        var melhorEstrategia = estrategias.First();

        foreach (var estrategia in estrategias)
        {
            var score = AvaliarEstrategia(
                estrategia,
                pato,
                distancia,
                altitude,
                vulnerabilidadesIdentificadas);

            if (score > melhorScore)
            {
                melhorScore = score;
                melhorEstrategia = estrategia;
            }
        }

        return new EstrategiaAtaque
        {
            Nome = melhorEstrategia,
            ScoreEfetividade = melhorScore,
            Descricao = _regrasAtaque[melhorEstrategia].Descricao,
            SequenciaAcoes = GerarSequenciaAcoes(melhorEstrategia, distancia, altitude)
        };
    }

    private double AvaliarEstrategia(
        string estrategia,
        PatoPrimordial pato,
        double distancia,
        double altitude,
        IEnumerable<string> vulnerabilidades)
    {
        var pesoBase = _regrasAtaque[estrategia].Peso;
        var scoreDistancia = CalcularScoreDistancia(distancia);
        var scoreAltitude = CalcularScoreAltitude(altitude);
        var scoreVulnerabilidades = CalcularScoreVulnerabilidades(estrategia, vulnerabilidades);

        return pesoBase * (scoreDistancia + scoreAltitude + scoreVulnerabilidades) / 3.0;
    }

    private static double CalcularScoreDistancia(double distancia)
    {
        return distancia switch
        {
            < 10 => 1.0,  // Distância ideal
            < 20 => 0.8,  // Boa distância
            < 30 => 0.6,  // Distância aceitável
            _ => 0.4      // Distância muito grande
        };
    }

    private static double CalcularScoreAltitude(double altitude)
    {
        return altitude switch
        {
            < 50 => 0.9,  // Baixa altitude, boa para ataques precisos
            < 100 => 0.7, // Altitude média
            < 200 => 0.5, // Alta altitude
            _ => 0.3      // Altitude muito alta
        };
    }

    private static double CalcularScoreVulnerabilidades(
        string estrategia,
        IEnumerable<string> vulnerabilidades)
    {
        var listaVulnerabilidades = vulnerabilidades.ToList();
        if (!listaVulnerabilidades.Any()) return 0.5;

        var scoreVulnerabilidades = 0.0;
        foreach (var vulnerabilidade in listaVulnerabilidades)
        {
            var bonus = estrategia switch
            {
                "AtaqueFrontal" when vulnerabilidade.Contains("frontal") => 0.3,
                "AtaqueFlancos" when vulnerabilidade.Contains("lateral") => 0.3,
                "AtaqueSurpresa" when vulnerabilidade.Contains("desprotegido") => 0.3,
                "AtaqueConjunto" when vulnerabilidade.Contains("individual") => 0.3,
                _ => 0.1
            };
            scoreVulnerabilidades += bonus;
        }

        return Math.Min(1.0, scoreVulnerabilidades);
    }

    private static List<string> GerarSequenciaAcoes(
        string estrategia,
        double distancia,
        double altitude)
    {
        var acoes = new List<string>();

        switch (estrategia)
        {
            case "AtaqueFrontal":
                acoes.Add("Ajustar altitude para confronto direto");
                acoes.Add("Acelerar para velocidade máxima");
                acoes.Add("Alinhar com o alvo");
                acoes.Add("Executar ataque frontal");
                break;

            case "AtaqueFlancos":
                acoes.Add("Posicionar-se lateralmente");
                acoes.Add("Executar manobra em zigue-zague");
                acoes.Add("Alternar entre flancos");
                acoes.Add("Atacar ponto fraco exposto");
                break;

            case "AtaqueSurpresa":
                acoes.Add("Reduzir assinatura de radar");
                acoes.Add("Aproximar pela área cega");
                acoes.Add("Aguardar momento oportuno");
                acoes.Add("Executar ataque rápido");
                break;

            case "AtaqueConjunto":
                acoes.Add("Sincronizar com drones próximos");
                acoes.Add("Distribuir posições táticas");
                acoes.Add("Coordenar timing de ataque");
                acoes.Add("Executar ataque simultâneo");
                break;
        }

        return acoes;
    }
}

public class EstrategiaAtaque
{
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public double ScoreEfetividade { get; set; }
    public List<string> SequenciaAcoes { get; set; } = new();
}