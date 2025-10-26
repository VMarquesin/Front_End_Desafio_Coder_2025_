using System;
using System.Linq;
using FluentAssertions;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Services;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Services;

public class AnalisadorVulnerabilidadesTests
{
    private readonly AnalisadorVulnerabilidades _analisador;

    public AnalisadorVulnerabilidadesTests()
    {
        _analisador = new AnalisadorVulnerabilidades();
    }

    [Fact]
    public void AnalisarPontosFracos_PatoGrande_DeveIdentificarVulnerabilidadeTamanho()
    {
        // Arrange
        var pato = CriarPatoTeste(
            altura: new Comprimento(150, Domain.Enums.UnidadeComprimento.Centimetro));

        // Act
        var vulnerabilidades = _analisador.AnalisarPontosFracos(pato).ToList();

        // Assert
        vulnerabilidades.Should().NotBeEmpty();
        vulnerabilidades.Should().Contain(v => v.Tipo == "Tamanho");
        var vulnTamanho = vulnerabilidades.First(v => v.Tipo == "Tamanho");
        vulnTamanho.ScoreEfetividade.Should().BeGreaterThan(80);
        vulnTamanho.TaticasRecomendadas.Should().Contain("AtaqueSuperior");
    }

    [Fact]
    public void AnalisarPontosFracos_PatoPesado_DeveIdentificarVulnerabilidadePeso()
    {
        // Arrange
        var pato = CriarPatoTeste(
            peso: new Massa(8000, Domain.Enums.UnidadeMassa.Grama));

        // Act
        var vulnerabilidades = _analisador.AnalisarPontosFracos(pato).ToList();

        // Assert
        vulnerabilidades.Should().NotBeEmpty();
        vulnerabilidades.Should().Contain(v => v.Tipo == "Peso");
        var vulnPeso = vulnerabilidades.First(v => v.Tipo == "Peso");
        vulnPeso.ScoreEfetividade.Should().BeGreaterThan(60);
        vulnPeso.TaticasRecomendadas.Should().Contain("CercoTatico");
    }

    [Fact]
    public void AnalisarPontosFracos_PatoEmTranse_DeveIdentificarVulnerabilidadeEstado()
    {
        // Arrange
        var pato = CriarPatoTeste(
            status: Domain.Enums.StatusHibernacao.Transe,
            bpm: 45);

        // Act
        var vulnerabilidades = _analisador.AnalisarPontosFracos(pato).ToList();

        // Assert
        vulnerabilidades.Should().NotBeEmpty();
        vulnerabilidades.Should().Contain(v => v.Tipo == "Estado");
        var vulnEstado = vulnerabilidades.First(v => v.Tipo == "Estado");
        vulnEstado.ScoreEfetividade.Should().BeGreaterThan(70);
        vulnEstado.TaticasRecomendadas.Should().Contain("AproximacaoSilenciosa");
    }

    private static PatoPrimordial CriarPatoTeste(
        Comprimento? altura = null,
        Massa? peso = null,
        Domain.Enums.StatusHibernacao status = Domain.Enums.StatusHibernacao.Desperto,
        int? bpm = null)
    {
        var coordenada = new Coordenada(-23.5505, -46.6333);
        var precisao = new Comprimento(5.0, Domain.Enums.UnidadeComprimento.Metro);
        var local = new Localizacao("São Paulo", "Brasil", coordenada, precisao, "Teste");
        
        altura ??= new Comprimento(50.0, Domain.Enums.UnidadeComprimento.Centimetro);
        peso ??= new Massa(2500.0, Domain.Enums.UnidadeMassa.Grama);
        
        var poder = new SuperPoder("TesteVoo", "Voa muito rápido", "mobilidade");

        return new PatoPrimordial(
            id: Guid.NewGuid(),
            droneNumeroSerie: "DRONE-TEST",
            altura: altura,
            peso: peso,
            localizacao: local,
            status: status,
            batimentosPorMinuto: bpm,
            quantidadeMutacoes: 2,
            poder: poder,
            dataColetaUtc: DateTime.UtcNow);
    }
}