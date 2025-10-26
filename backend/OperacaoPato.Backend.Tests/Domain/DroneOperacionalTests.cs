using System;
using FluentAssertions;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Domain;

public class DroneOperacionalTests
{
    [Fact]
    public void DroneOperacional_DeveCriarComParametrosValidos()
    {
        // Arrange
        var bateria = new NivelRecurso(100, 100, "kWh");
        var combustivel = new NivelRecurso(50, 50, "L");
        var integridade = new NivelRecurso(100, 100, "%");
        var posicao = new Coordenada(-23.5505, -46.6333);

        // Act
        var drone = new DroneOperacional(
            "DRONE-TEST",
            "TestDrone",
            "Test Inc",
            "Test Country",
            bateria,
            combustivel,
            integridade,
            posicao);

        // Assert
        drone.NumeroSerie.Should().Be("DRONE-TEST");
        drone.Bateria.Should().Be(bateria);
        drone.Combustivel.Should().Be(combustivel);
        drone.IntegridadeFisica.Should().Be(integridade);
        drone.Posicao.Should().Be(posicao);
        drone.VelocidadeAtual.Should().Be(0);
        drone.AltitudeAtual.Should().Be(0);
    }

    [Fact]
    public void AtualizarPosicao_DeveCalcularConsumoCorretamente()
    {
        // Arrange
        var bateria = new NivelRecurso(100, 100, "kWh");
        var combustivel = new NivelRecurso(50, 50, "L");
        var integridade = new NivelRecurso(100, 100, "%");
        var posicaoInicial = new Coordenada(-23.5505, -46.6333);

        var drone = new DroneOperacional(
            "DRONE-TEST", "TestDrone", "Test Inc", "Test Country",
            bateria, combustivel, integridade, posicaoInicial);

        var novaPosicao = new Coordenada(-23.5605, -46.6433); // ~1.5km distância
        var timestamp = DateTime.UtcNow.AddMinutes(5);

        // Act
        drone.AtualizarPosicao(novaPosicao, 20.0, 100.0, timestamp);

        // Assert
        drone.Posicao.Should().Be(novaPosicao);
        drone.VelocidadeAtual.Should().Be(20.0);
        drone.AltitudeAtual.Should().Be(100.0);
        drone.Bateria.PorcentagemAtual.Should().BeLessThan(100);
        drone.Combustivel.PorcentagemAtual.Should().BeLessThan(100);
    }

    [Fact]
    public void PodeOperar_DeveDarFalsoBateriaEmNivelCritico()
    {
        // Arrange
        var bateria = new NivelRecurso(15, 100, "kWh"); // 15% - crítico
        var combustivel = new NivelRecurso(50, 50, "L");
        var integridade = new NivelRecurso(100, 100, "%");
        var posicao = new Coordenada(-23.5505, -46.6333);

        var drone = new DroneOperacional(
            "DRONE-TEST", "TestDrone", "Test Inc", "Test Country",
            bateria, combustivel, integridade, posicao);

        // Act & Assert
        drone.PodeOperar().Should().BeFalse();
    }

    [Fact]
    public void RegistrarDano_DeveAtualizarIntegridadeFisica()
    {
        // Arrange
        var bateria = new NivelRecurso(100, 100, "kWh");
        var combustivel = new NivelRecurso(50, 50, "L");
        var integridade = new NivelRecurso(100, 100, "%");
        var posicao = new Coordenada(-23.5505, -46.6333);

        var drone = new DroneOperacional(
            "DRONE-TEST", "TestDrone", "Test Inc", "Test Country",
            bateria, combustivel, integridade, posicao);

        // Act
        drone.RegistrarDano(30); // 30% de dano

        // Assert
        drone.IntegridadeFisica.PorcentagemAtual.Should().Be(70);
    }
}