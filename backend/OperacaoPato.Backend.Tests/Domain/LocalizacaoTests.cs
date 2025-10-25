using System;
using System.Globalization;
using FluentAssertions;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Domain
{
    public class LocalizacaoTests
    {
        public LocalizacaoTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public void Deve_Criar_Localizacao_Quando_Precisao_Valida()
        {
            var coord = new Coordenada(10.0, 20.0);
            var precisao = new Comprimento(4.0, UnidadeComprimento.Centimetro); // 4 cm
            var loc = new Localizacao("Manaus", "Brasil", coord, precisao, "Pico da Neblina");

            loc.Cidade.Should().Be("Manaus");
            loc.Pais.Should().Be("Brasil");
            loc.Coordenada.Should().Be(coord);
            loc.PontoReferencia.Should().Be("Pico da Neblina");
        }

        [Fact]
        public void Deve_Lancar_Quando_Precisao_Muito_Pequena()
        {
            var coord = new Coordenada(0.0, 0.0);
            var precisao = new Comprimento(3.0, UnidadeComprimento.Centimetro); // 3 cm -> inválido

            Action act = () => new Localizacao("Cidade", "Pais", coord, precisao);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Deve_Lancar_Quando_Precisao_Muito_Grande()
        {
            var coord = new Coordenada(0.0, 0.0);
            var precisao = new Comprimento(31.0, UnidadeComprimento.Metro); // 31 m -> inválido

            Action act = () => new Localizacao("Cidade", "Pais", coord, precisao);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Coordenada_Latitude_Fora_Do_Intervalo_Deve_Lancar()
        {
            Action act = () => new Coordenada(100.0, 0.0);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Coordenada_Longitude_Fora_Do_Intervalo_Deve_Lancar()
        {
            Action act = () => new Coordenada(0.0, 200.0);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void PontoReferencia_Vazio_Deve_Ficar_Nulo()
        {
            var coord = new Coordenada(1.0, 1.0);
            var precisao = new Comprimento(10.0, UnidadeComprimento.Centimetro);
            var loc = new Localizacao("Cidade", "Pais", coord, precisao, "   ");

            loc.PontoReferencia.Should().BeNull();
        }

    }
}