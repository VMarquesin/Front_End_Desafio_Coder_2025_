using System;
using System.Collections.Generic;
using System.Linq;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Enums;

namespace OperacaoPato.Backend.Domain.Services;

public class AnalisadorVulnerabilidades
{
    public record VulnerabilidadeIdentificada(
        string Tipo,
        string Descricao,
        double ScoreEfetividade,
        IEnumerable<string> TaticasRecomendadas);

    public IEnumerable<VulnerabilidadeIdentificada> AnalisarPontosFracos(PatoPrimordial pato)
    {
        var vulnerabilidades = new List<VulnerabilidadeIdentificada>();

        // Análise de tamanho
        if (pato.ObterAlturaEm(UnidadeComprimento.Centimetro).Valor > 100)
        {
            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "Tamanho",
                "Alvo grande: vulnerável a ataques superiores",
                85,
                new[] { 
                    "AtaqueSuperior",
                    "LancamentoPedra",
                    "RedeCaptura"
                }
            ));
        }

        // Análise de peso/mobilidade
        var pesoGramas = pato.Peso.EmGramas();
        if (pesoGramas > 5000)
        {
            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "Peso",
                "Mobilidade reduzida devido ao peso elevado",
                70,
                new[] { 
                    "CercoTatico",
                    "CansacoProgressivo",
                    "ArmadilhaTerrestre"
                }
            ));
        }

        // Análise de status
        if (pato.Status == Enums.StatusHibernacao.Transe)
        {
            var efetividade = 60.0;
            if (pato.BatimentosPorMinuto.HasValue && pato.BatimentosPorMinuto.Value < 50)
                efetividade = 80.0;

            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "Estado",
                "Vulnerável durante estado de transe",
                efetividade,
                new[] { 
                    "AproximacaoSilenciosa",
                    "RedeEletromagnetica",
                    "SonoriferoNatural"
                }
            ));
        }

        // Análise de poder
        if (pato.Poder != null)
        {
            var classificacao = pato.Poder.Classificacao.ToLower();
            if (classificacao.Contains("ofens"))
            {
                vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                    "ContraPoder",
                    "Poder ofensivo: necessita defesa apropriada",
                    75,
                    new[] { 
                        "EscudoEnergetico",
                        "CampoDeflator",
                        "ContraAtaqueTatico"
                    }
                ));
            }
            else if (classificacao.Contains("defens"))
            {
                vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                    "ContraPoder",
                    "Poder defensivo: necessita sobrecarga",
                    65,
                    new[] { 
                        "AtaqueSustentado",
                        "SobrecargaEnergetica",
                        "DisruptorDefesa"
                    }
                ));
            }
        }

        // Análise de mutações
        if (pato.QuantidadeMutacoes > 3)
        {
            vulnerabilidades.Add(new VulnerabilidadeIdentificada(
                "Mutacao",
                "Alta quantidade de mutações: possível instabilidade",
                55,
                new[] { 
                    "EstabilizadorGenetico",
                    "CampoNormalizador",
                    "PulsoCalmante"
                }
            ));
        }

        return vulnerabilidades;
    }
}