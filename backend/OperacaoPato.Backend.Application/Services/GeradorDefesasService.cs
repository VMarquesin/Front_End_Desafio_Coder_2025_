using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Enums;

namespace OperacaoPato.Backend.Application.Services;

public class GeradorDefesasService
{
    private static readonly Random _random = new();
    
    private readonly Dictionary<string, double> _efetividadeDefesas = new()
    {
        { "CampoForca", 0.8 },
        { "ContraAtaque", 0.7 },
        { "Evasao", 0.6 },
        { "BlindarPontoFraco", 0.9 },
        { "DissiparEnergia", 0.75 }
    };

    public DefesaGerada GerarDefesaAleatoria(SuperPoder superPoder)
    {
        var defesasDisponiveis = _efetividadeDefesas.Keys.ToList();
        var defesaEscolhida = defesasDisponiveis[_random.Next(defesasDisponiveis.Count)];
        var efetividade = _efetividadeDefesas[defesaEscolhida];
        
        // Ajusta efetividade baseado no tipo do superpoder
        efetividade *= CalcularModificadorPoder(superPoder);

        return new DefesaGerada
        {
            Nome = defesaEscolhida,
            Efetividade = efetividade,
            DuracaoSegundos = _random.Next(10, 31),
            CustoEnergia = _random.Next(20, 51)
        };
    }

    private static double CalcularModificadorPoder(SuperPoder poder)
    {
        var nivelBase = poder.Classificacao.ToLower() switch
        {
            "baixo" => 1.2,
            "medio" => 1.0,
            "alto" => 0.8,
            _ => 1.0
        };

        return nivelBase * (1.0 + _random.NextDouble() * 0.2);
    }
}

public class DefesaGerada
{
    public string Nome { get; set; } = "";
    public double Efetividade { get; set; }
    public int DuracaoSegundos { get; set; }
    public int CustoEnergia { get; set; }
}