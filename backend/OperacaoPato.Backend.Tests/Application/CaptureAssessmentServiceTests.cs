using System;
using FluentAssertions;
using OperacaoPato.Backend.Application.Services;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Backend.Domain.Enums;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Application
{
    public class CaptureAssessmentServiceTests
    {
        [Fact]
        public void Assess_Returns_Result_For_Sample_Pato()
        {
            // Arrange
            var coordenada = new Coordenada(-23.55052, -46.633308);
            var precisao = new Comprimento(5.0, UnidadeComprimento.Metro);
            var local = new Localizacao("São Paulo", "Brasil", coordenada, precisao, "Parque Ibirapuera");
            var altura = new Comprimento(55.0, UnidadeComprimento.Centimetro);
            var peso = new Massa(3000.0, UnidadeMassa.Grama);
            var poder = new OperacaoPato.Backend.Domain.Entities.SuperPoder("Voo Supersônico", "Voa muito rápido", "mobilidade");

            var pato = new PatoPrimordial(
                droneNumeroSerie: "DRONE-TEST",
                altura: altura,
                peso: peso,
                localizacao: local,
                status: StatusHibernacao.Transe,
                batimentosPorMinuto: 45,
                quantidadeMutacoes: 2,
                poder: poder,
                dataColetaUtc: DateTime.UtcNow);

            var service = new CaptureAssessmentService();

            // Act
            var result = service.Assess(pato);

            // Assert basic expectations
            result.Should().NotBeNull();
            result.PatoId.Should().Be(pato.Id);
            result.CaptureRiskScore.Should().BeInRange(0, 100);
            result.OperationalCostScore.Should().BeInRange(0, 100);
            result.ScientificValueScore.Should().BeInRange(0, 100);
            result.RequiredForceLevel.Should().NotBeNullOrWhiteSpace();
            result.Recommendation.Should().NotBeNullOrWhiteSpace();
        }
    }
}
