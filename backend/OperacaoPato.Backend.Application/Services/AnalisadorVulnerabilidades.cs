using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Enums;

namespace OperacaoPato.Backend.Application.Services;

public class AnalisadorVulnerabilidades
{
    private readonly GeradorDefesasService _geradorDefesas;
    private readonly TaticaAtaqueService _taticaAtaque;

    public AnalisadorVulnerabilidades(
        GeradorDefesasService geradorDefesas,
        TaticaAtaqueService taticaAtaque)
    {
        _geradorDefesas = geradorDefesas;
        _taticaAtaque = taticaAtaque;
    }

    public IEnumerable<VulnerabilidadeIdentificada> AnalisarPontosFracos(PatoPrimordial pato)
    {
        var vulnerabilidades = new List<VulnerabilidadeIdentificada>();

        // Análise física
        var alturaEmCm = pato.ObterAlturaEm(UnidadeComprimento.Centimetro).Valor;
        if (alturaEmCm > 100)
        {
            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "Física",
                "Tamanho acima do normal - movimentação reduzida",
                0.8,
                new[] { "AtaqueFlancos", "AtaqueConjunto" }));
        }

        // Análise de peso/mobilidade
        var pesoGramas = pato.ObterPesoEm(UnidadeMassa.Grama);
        if (pesoGramas > 5000)
        {
            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "Mobilidade",
                "Peso elevado - agilidade comprometida",
                0.7,
                new[] { "CercoTatico", "CansacoProgressivo" }));
        }

        // Análise do superpoder
        {
            var defesa = _geradorDefesas.GerarDefesaAleatoria(pato.Poder);
            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "SuperPoder",
                $"Vulnerável a {defesa.Nome} contra {pato.Poder.Nome}",
                defesa.Efetividade,
                new[] { "AtaqueSurpresa", "AtaqueFrontal" }));
        }

        return vulnerabilidades;
    }
}

public record VulnerabilidadeIdentificada(
    string Tipo,
    string Descricao,
    double ScoreEfetividade,
    IEnumerable<string> TaticasRecomendadas);