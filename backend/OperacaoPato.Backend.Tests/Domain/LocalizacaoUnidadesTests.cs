using System;
using FluentAssertions;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Domain
{
    public class LocalizacaoUnidadesTests
    {
        [Fact]
        public void Precisao_Em_Centimetros_Deve_Ser_Valida()
        {
            var coord = new Coordenada(-3.0, -60.0);
            var precisao = new Comprimento(4.0, UnidadeComprimento.Centimetro); // 4 cm
            var loc = new Localizacao("Manaus", "BR", coord, precisao);

            loc.Cidade.Should().Be("Manaus");
        }

        [Fact]
        public void Precisao_Em_Polegadas_Equivalente_A_4cm_Deve_Ser_Valida()
        {
            var coord = new Coordenada(0.0, 0.0);
            // 4 cm ≈ 1.5748 in
            var precisao = new Comprimento(1.6, UnidadeComprimento.Polegada);
            Action act = () => new Localizacao("Cidade", "Pais", coord, precisao);

            act.Should().NotThrow();
        }

        [Fact]
        public void Precisao_Em_Pes_Equivalente_A_4cm_Deve_Ser_Valida()
        {
            var coord = new Coordenada(10.0, 10.0);
            // 4 cm ≈ 0.1312336 ft
            var precisao = new Comprimento(0.1312336, UnidadeComprimento.Pe);
            Action act = () => new Localizacao("Cidade", "Pais", coord, precisao);

            act.Should().NotThrow();
        }

        [Fact]
        public void Precisao_Em_Jardas_Abaixo_De_30m_Deve_Ser_Valida_E_Acima_Deve_Falhar()
        {
            var coord = new Coordenada(1.0, 1.0);

            // 32.8 yd ≈ 29.99 m -> deve ser válida
            var precisaoValida = new Comprimento(32.8, UnidadeComprimento.Jarda);
            Action actValida = () => new Localizacao("Cidade", "Pais", coord, precisaoValida);
            actValida.Should().NotThrow();

            // 33 yd ≈ 30.1752 m -> deve falhar (> 30 m)
            var precisaoInvalida = new Comprimento(33.0, UnidadeComprimento.Jarda);
            Action actInvalida = () => new Localizacao("Cidade", "Pais", coord, precisaoInvalida);
            actInvalida.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Precisao_Em_Quilometro_Exatamente_30m_Deve_Ser_Valida_E_Levemente_Maior_Deve_Falhar()
        {
            var coord = new Coordenada(1.0, 1.0);

            // 0.03 km == 30 m -> permitido
            var preciso30m = new Comprimento(0.03, UnidadeComprimento.Quilometro);
            Action act = () => new Localizacao("Cidade", "Pais", coord, preciso30m);
            act.Should().NotThrow();

            // 0.031 km == 31 m -> inválido
            var preciso31m = new Comprimento(0.031, UnidadeComprimento.Quilometro);
            Action act31 = () => new Localizacao("Cidade", "Pais", coord, preciso31m);
            act31.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Precisao_Muito_Pequena_Em_Polegada_Deve_Falhar()
        {
            var coord = new Coordenada(0.0, 0.0);
            // 0.5 in ≈ 1.27 cm -> menor que 4 cm -> inválido
            var precisao = new Comprimento(0.5, UnidadeComprimento.Polegada);
            Action act = () => new Localizacao("Cidade", "Pais", coord, precisao);
            act.Should().Throw<ArgumentException>();
        }
    }
}