using System;
using System.Linq;
using FluentAssertions;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Services;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Services;

public class ControladorVooTests
{
    private readonly ControladorVooService _controlador;
    private readonly DroneOperacional _drone;

    public ControladorVooTests()
    {
        _controlador = new ControladorVooService();
        
        var bateria = new NivelRecurso(100, 100, "kWh");
        var combustivel = new NivelRecurso(50, 50, "L");
        var integridade = new NivelRecurso(100, 100, "%");
        var posicaoInicial = new Coordenada(-23.5505, -46.6333);

        _drone = new DroneOperacional(
            "DRONE-TEST", "TestDrone", "Test Inc", "Test Country",
            bateria, combustivel, integridade, posicaoInicial);
    }

    [Fact]
    public void CalcularInstrucaoVoo_DeveGerarInstrucaoValida()
    {
        // Arrange
        var origem = new Coordenada(-23.5505, -46.6333);
        var destino = new Coordenada(-23.5605, -46.6433);
        
        // Act
        var instrucao = _controlador.CalcularInstrucaoVoo(
            origem, destino, 0, 0, 50);

        // Assert
        instrucao.Should().NotBeNull();
        instrucao.Destino.Should().Be(destino);
        instrucao.VelocidadeAlvo.Should().BeGreaterThan(0);
        instrucao.AltitudeAlvo.Should().Be(50);
        instrucao.TempoEstimado.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Fact]
    public void ExecutarInstrucao_DeveMoverDroneECalcularConsumo()
    {
        // Arrange
        var destino = new Coordenada(-23.5605, -46.6433);
        var instrucao = new ControladorVooService.InstrucaoVoo(
            destino, 20, 50, TimeSpan.FromMinutes(5));

        // Act
        var resultado = _controlador.ExecutarInstrucao(_drone, instrucao);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Sucesso.Should().BeTrue();
        resultado.DistanciaPercorrida.Should().BeGreaterThan(0);
        resultado.EnergiaConsumida.Should().BeGreaterThan(0);
        resultado.TempoGasto.Should().Be(instrucao.TempoEstimado);
    }

    [Fact]
    public void ExecutarInstrucao_ComDroneSemRecursos_DeveFalhar()
    {
        // Arrange
        var bateriaCritica = new NivelRecurso(5, 100, "kWh");
        var combustivel = new NivelRecurso(50, 50, "L");
        var integridade = new NivelRecurso(100, 100, "%");
        var posicao = new Coordenada(-23.5505, -46.6333);

        var droneCritico = new DroneOperacional(
            "DRONE-TEST", "TestDrone", "Test Inc", "Test Country",
            bateriaCritica, combustivel, integridade, posicao);

        var destino = new Coordenada(-23.5605, -46.6433);
        var instrucao = new ControladorVooService.InstrucaoVoo(
            destino, 20, 50, TimeSpan.FromMinutes(5));

        // Act
        var resultado = _controlador.ExecutarInstrucao(droneCritico, instrucao);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Sucesso.Should().BeFalse();
        resultado.Mensagem.Should().Contain("não tem recursos suficientes");
    }
}