using System;
using System.Linq;
using System.Text;

namespace OperacaoPato.Backend.Domain.Services // Ajuste o namespace se necessário
{
    public static class GeradorDna
    {
        private static readonly string[] Bases = { "A", "T", "C", "G" };
        private static readonly string[] NomesMutacoes = { 
            "ASAS-DE-ACO", "OLHOS-LASER", "PELE-REFORCADA", "GRITO-SONICO", 
            "CAMUFLAGEM-ATIVA", "GARRAS-RETRATEIS", "PROPULSAO-A-JATO", "MENTE-COLETIVA" 
        };
        private static Random random = new Random();

        public static string GerarSequencia(int quantidadeMutacoes)
        {
            if (quantidadeMutacoes <= 0) return "Sequência base: ATC-GGT-CAA...";

            var sb = new StringBuilder();
            int tamanhoBase = 150 + (quantidadeMutacoes * 5); // Sequência fica maior com mais mutações
            int mutacoesInseridas = 0;

            for (int i = 0; i < tamanhoBase; i++)
            {
                // Chance de inserir uma mutação
                if (mutacoesInseridas < quantidadeMutacoes && random.NextDouble() < 0.1) // 10% de chance por base
                {
                    mutacoesInseridas++;
                    string nomeMutacao = NomesMutacoes[random.Next(NomesMutacoes.Length)];
                    sb.Append($"[MUT-{mutacoesInseridas:D2}:{nomeMutacao}]-");
                    i += 5; // Pula algumas bases para a mutação
                }
                else
                {
                    // Adiciona um bloco de 3 bases
                    sb.Append(Bases[random.Next(Bases.Length)]);
                    sb.Append(Bases[random.Next(Bases.Length)]);
                    sb.Append(Bases[random.Next(Bases.Length)]);
                    if (i < tamanhoBase - 1) sb.Append("-");
                    i += 2; // Avança o loop
                }
            }

            // Garante que todas as mutações foram inseridas se a chance não foi suficiente
            while(mutacoesInseridas < quantidadeMutacoes)
            {
                 mutacoesInseridas++;
                 string nomeMutacao = NomesMutacoes[random.Next(NomesMutacoes.Length)];
                 sb.Append($"- [MUT-{mutacoesInseridas:D2}:{nomeMutacao}]");
            }


            return sb.ToString();
        }
    }
}