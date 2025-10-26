using System;
using System.Collections.Generic;
using System.Linq;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Domain.Services;

public class GeradorDefesasService
{
    private readonly Random _random = new();

    public record DefesaGerada(
        string Nome,
        string Descricao,
        double EfetividadeEstimada,
        double CustoEnergia,
        TimeSpan DuracaoEfeito);

    private readonly Dictionary<string, List<DefesaTemplate>> _catalogoDefesas = new()
    {
        ["chocolate"] = new()
        {
            new("Brigada Infantil", "Teleporta crianças de festas infantis para atirar brigadeiros", 0.8, 50),
            new("Chuva de Chocolate", "Forma nuvem de gotas de chocolate derretido", 0.7, 40),
            new("Barreira Cacau", "Cria uma parede de chocolate maciço", 0.6, 35)
        },
        ["agua"] = new()
        {
            new("Escudo de Vapor", "Cria uma barreira de vapor superaquecido", 0.75, 45),
            new("Campo Desidratante", "Gera área que absorve umidade", 0.65, 40),
            new("Bolha de Ar", "Envolve o drone em uma bolha de ar pressurizado", 0.7, 35)
        },
        ["eletricidade"] = new()
        {
            new("Gaiola de Faraday", "Envolve o drone em malha condutora", 0.85, 55),
            new("Pulso Terra", "Dispersa cargas elétricas para o solo", 0.7, 40),
            new("Campo Isolante", "Cria barreira de material isolante", 0.75, 45)
        },
        ["fogo"] = new()
        {
            new("Escudo Térmico", "Projeta campo de dispersão de calor", 0.8, 50),
            new("Cortina Criogênica", "Gera névoa super-resfriada", 0.75, 45),
            new("Espuma Antichamas", "Dispara espuma isolante especial", 0.7, 40)
        },
        ["som"] = new()
        {
            new("Bolha Silenciosa", "Cria zona de cancelamento sonoro", 0.75, 45),
            new("Campo Harmônico", "Gera contraondas neutralizantes", 0.7, 40),
            new("Barreira Acústica", "Forma parede que absorve ondas sonoras", 0.65, 35)
        },
        ["vento"] = new()
        {
            new("Âncora Gravitacional", "Aumenta atração gravitacional local", 0.8, 50),
            new("Estabilizador Inercial", "Neutraliza efeitos de rajadas", 0.75, 45),
            new("Escudo Aerodinâmico", "Cria perfil que desvia fluxo de ar", 0.7, 40)
        }
    };

    private record DefesaTemplate(string Nome, string Descricao, double EfetividadeBase, double EnergiaBase);

    public DefesaGerada GerarDefesaAleatoria(SuperPoder poderInimigo, double intensidadeDesejada)
    {
        var tipoDefesa = IdentificarTipoDefesa(poderInimigo);
        var templates = _catalogoDefesas.GetValueOrDefault(tipoDefesa) ?? _catalogoDefesas.First().Value;

        // Escolhe template aleatório dando peso maior para os mais efetivos
        var templateIndex = EscolherIndexPonderado(templates.Count);
        var template = templates[templateIndex];

        // Ajusta efetividade e custo baseado na intensidade desejada
        var fatorIntensidade = Math.Min(1.5, Math.Max(0.5, intensidadeDesejada));
        var efetividade = template.EfetividadeBase * fatorIntensidade;
        var custoEnergia = template.EnergiaBase * fatorIntensidade;

        // Duração do efeito é inversamente proporcional à efetividade
        var duracaoSegundos = 30 * (2 - template.EfetividadeBase);

        return new DefesaGerada(
            template.Nome,
            template.Descricao,
            Math.Round(efetividade * 100, 1),
            Math.Round(custoEnergia, 1),
            TimeSpan.FromSeconds(duracaoSegundos));
    }

    private string IdentificarTipoDefesa(SuperPoder poder)
    {
        var desc = poder.Descricao.ToLower();
        var nome = poder.Nome.ToLower();

        if (desc.Contains("chocolate") || desc.Contains("doce"))
            return "chocolate";
        if (desc.Contains("agua") || desc.Contains("liquido"))
            return "agua";
        if (desc.Contains("eletric") || desc.Contains("raio"))
            return "eletricidade";
        if (desc.Contains("fogo") || desc.Contains("calor"))
            return "fogo";
        if (desc.Contains("som") || desc.Contains("sonic"))
            return "som";
        if (desc.Contains("vento") || desc.Contains("ar"))
            return "vento";

        // Tenta pelo nome se não achou na descrição
        if (nome.Contains("chocolate") || nome.Contains("doce"))
            return "chocolate";
        if (nome.Contains("agua") || nome.Contains("liquido"))
            return "agua";
        if (nome.Contains("eletric") || nome.Contains("raio"))
            return "eletricidade";
        if (nome.Contains("fogo") || nome.Contains("calor"))
            return "fogo";
        if (nome.Contains("som") || nome.Contains("sonic"))
            return "som";
        if (nome.Contains("vento") || nome.Contains("ar"))
            return "vento";

        // Se não identificou, retorna aleatório
        var tipos = _catalogoDefesas.Keys.ToList();
        return tipos[_random.Next(tipos.Count)];
    }

    private int EscolherIndexPonderado(int count)
    {
        // Dá mais peso para os primeiros itens (mais efetivos)
        var total = (count * (count + 1)) / 2;
        var r = _random.Next(1, total + 1);
        var sum = 0;
        
        for (var i = count; i >= 1; i--)
        {
            sum += i;
            if (r <= sum)
                return count - i;
        }

        return 0;
    }
}