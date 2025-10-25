using System;
using FluentAssertions;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Backend.Domain.Enums;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Domain
{
    public class PatoPrimordialTests
    {
        private static Localizacao CriarLocalizacaoValida()
        {
            var coordenada = new Coordenada(0.0, 0.0);
            var precisao = new Comprimento(1.0, UnidadeComprimento.Metro);
            return new Localizacao("Cidade", "Pais", coordenada, precisao, "Ponto");
        }

        [Fact]
        public void Deve_Preservar_Unidade_Original_De_Altura_e_Peso()
        {
            var altura = new Comprimento(5.0, UnidadeComprimento.Jarda);
            var peso = new Massa(10.0, UnidadeMassa.Libra);
            var loc = CriarLocalizacaoValida();
            var poder = new SuperPoder("Tempestade Elétrica", "Gera descargas elétricas", "bélico");

            var pato = new PatoPrimordial(
                droneNumeroSerie: "DRONE-001",
                altura: altura,
                peso: peso,
                localizacao: loc,
                status: StatusHibernacao.Desperto,
                batimentosPorMinuto: null,
                quantidadeMutacoes: 3,
                poder: poder);

            pato.Altura.Valor.Should().Be(5.0);
            pato.Altura.UnidadeComprimento.Should().Be(UnidadeComprimento.Jarda);

            pato.Peso.Em(UnidadeMassa.Libra).Should().Be(10.0);
            pato.Peso.UnidadeMassa.Should().Be(UnidadeMassa.Libra);

            pato.MostrarAltura().Should().Be(altura.ToString());
            pato.MostrarPeso(usarUnidadeOriginal: true).Should().Be(peso.ToString());
        }

        [Fact]
        public void Deve_Converter_Altura_e_Peso_Sob_Demanda()
        {
            var altura = new Comprimento(5.0, UnidadeComprimento.Jarda); // 5 yd
            var peso = new Massa(10.0, UnidadeMassa.Libra); // 10 lb
            var loc = CriarLocalizacaoValida();
            var poder = new SuperPoder("Laser Ocular", "Dispara raios", "raro");

            var pato = new PatoPrimordial(
                droneNumeroSerie: "DRONE-002",
                altura: altura,
                peso: peso,
                localizacao: loc,
                status: StatusHibernacao.Desperto,
                batimentosPorMinuto: null,
                quantidadeMutacoes: 1,
                poder: poder);

            var alturaEmMetros = pato.ObterAlturaEm(UnidadeComprimento.Metro);
            alturaEmMetros.Valor.Should().BeApproximately(5.0 * 0.9144, 1e-6);

            var pesoEmGramas = pato.ObterPesoEm(UnidadeMassa.Grama);
            pesoEmGramas.Should().BeApproximately(10.0 * 453.59237, 1e-6);
        }

        [Fact]
        public void Transe_e_HibernacaoProfunda_Exigem_Batimentos()
        {
            var altura = new Comprimento(100.0, UnidadeComprimento.Centimetro);
            var peso = new Massa(5000.0, UnidadeMassa.Grama);
            var loc = CriarLocalizacaoValida();
            var poder = new SuperPoder("Força Bruta", "Aumenta força", "alto risco");

            Action actTranse = () => new PatoPrimordial(
                droneNumeroSerie: "DRONE-003",
                altura: altura,
                peso: peso,
                localizacao: loc,
                status: StatusHibernacao.Transe,
                batimentosPorMinuto: null,
                quantidadeMutacoes: 0,
                poder: poder);

            Action actHiber = () => new PatoPrimordial(
                droneNumeroSerie: "DRONE-004",
                altura: altura,
                peso: peso,
                localizacao: loc,
                status: StatusHibernacao.HibernacaoProfunda,
                batimentosPorMinuto: null,
                quantidadeMutacoes: 0,
                poder: poder);

            actTranse.Should().Throw<ArgumentException>();
            actHiber.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void QuantidadeMutacoes_Negativa_Deve_Lancar()
        {
            var altura = new Comprimento(50.0, UnidadeComprimento.Centimetro);
            var peso = new Massa(2000.0, UnidadeMassa.Grama);
            var loc = CriarLocalizacaoValida();
            var poder = new SuperPoder("Camuflagem", "Fica invisível", "comum");

            Action act = () => new PatoPrimordial(
                droneNumeroSerie: "DRONE-005",
                altura: altura,
                peso: peso,
                localizacao: loc,
                status: StatusHibernacao.Desperto,
                batimentosPorMinuto: null,
                quantidadeMutacoes: -1,
                poder: poder);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Altura_Ou_Peso_Nao_Positive_Deve_Lancar()
        {
            var loc = CriarLocalizacaoValida();
            var poder = new SuperPoder("Explosão", "Explode", "extremo");

            var alturaZero = new Comprimento(0.0, UnidadeComprimento.Metro);
            var pesoValido = new Massa(1000.0, UnidadeMassa.Grama);

            Action actAlturaZero = () => new PatoPrimordial(
                droneNumeroSerie: "DRONE-006",
                altura: alturaZero,
                peso: pesoValido,
                localizacao: loc,
                status: StatusHibernacao.Desperto,
                batimentosPorMinuto: null,
                quantidadeMutacoes: 0,
                poder: poder);

            actAlturaZero.Should().Throw<ArgumentException>();

            var alturaValida = new Comprimento(100.0, UnidadeComprimento.Centimetro);
            var pesoZero = new Massa(0.0, UnidadeMassa.Grama);

            Action actPesoZero = () => new PatoPrimordial(
                droneNumeroSerie: "DRONE-007",
                altura: alturaValida,
                peso: pesoZero,
                localizacao: loc,
                status: StatusHibernacao.Desperto,
                batimentosPorMinuto: null,
                quantidadeMutacoes: 0,
                poder: poder);

            actPesoZero.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void DroneNumeroSerie_Armazenado_Como_Id()
        {
            var altura = new Comprimento(150.0, UnidadeComprimento.Centimetro);
            var peso = new Massa(3000.0, UnidadeMassa.Grama);
            var loc = CriarLocalizacaoValida();
            var poder = new SuperPoder("Voo", "Permite voar", "raro");

            var pato = new PatoPrimordial(
                droneNumeroSerie: "SN-ABC-123",
                altura: altura,
                peso: peso,
                localizacao: loc,
                status: StatusHibernacao.Desperto,
                batimentosPorMinuto: 60,
                quantidadeMutacoes: 2,
                poder: poder);

            pato.DroneNumeroSerie.Should().Be("SN-ABC-123");
            pato.ToString().Should().Contain("SN-ABC-123");
        }
    }
}