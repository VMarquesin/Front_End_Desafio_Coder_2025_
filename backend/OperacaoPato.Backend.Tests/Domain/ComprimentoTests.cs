using System;
using System.Globalization;
using FluentAssertions;
using OperacaoPato.Domain.ValueObjects;
using Xunit;

namespace OperacaoPato.Backend.Tests.Domain
{
    public class ComprimentoTests
    {
        public ComprimentoTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public void Criar_Comprimento_Com_ValorNegativo_Deve_Lancar_ArgumentException()
        {
            Action act = () => new Comprimento(-1, UnidadeComprimento.Metro);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(1.0, 0, 1, 100.0)]
        [InlineData(1.0, 2, 0, 1000.0)]
        [InlineData(12.0, 3, 1, 12.0 * 2.54)]
        [InlineData(3.0, 4, 0, 3.0 * 0.3048)]
        public void ConverterPara_Deve_Retornar_ValorEsperado(double valor, int origemInt, int destinoInt, double esperado)
        {
            var origem = (UnidadeComprimento)origemInt;
            var destino = (UnidadeComprimento)destinoInt;

            var c = new Comprimento(valor, origem);
            var convertido = c.ConverterPara(destino);

            convertido.UnidadeComprimento.Should().Be(destino);
            convertido.Valor.Should().BeApproximately(esperado, 1e-6);
        }

        [Fact]
        public void Somar_Deve_Retornar_Soma_No_Mesmo_Unidade()
        {
            var a = new Comprimento(1.0, UnidadeComprimento.Metro);
            var b = new Comprimento(50.0, UnidadeComprimento.Centimetro); // 0.5 m

            var soma = a.Somar(b);

            soma.UnidadeComprimento.Should().Be(UnidadeComprimento.Metro);
            soma.Valor.Should().BeApproximately(1.5, 1e-9);
        }

        [Fact]
        public void Subtrair_Deve_Retornar_Resultado_Correto()
        {
            var a = new Comprimento(2.0, UnidadeComprimento.Metro);
            var b = new Comprimento(50.0, UnidadeComprimento.Centimetro); // 0.5 m

            var resultado = a.Subtrair(b);

            resultado.UnidadeComprimento.Should().Be(UnidadeComprimento.Metro);
            resultado.Valor.Should().BeApproximately(1.5, 1e-9);
        }

        [Fact]
        public void Subtrair_Quando_Resultar_Negativo_Deve_Lancar_InvalidOperationException()
        {
            var a = new Comprimento(0.5, UnidadeComprimento.Metro);
            var b = new Comprimento(1.0, UnidadeComprimento.Metro);

            Action act = () => a.Subtrair(b);
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Equals_Deve_Considerar_Medidas_Equivalentes_Iguais()
        {
            var a = new Comprimento(100.0, UnidadeComprimento.Centimetro);
            var b = new Comprimento(1.0, UnidadeComprimento.Metro);

            a.Equals(b).Should().BeTrue();
            b.Equals(a).Should().BeTrue();
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void ToString_Deve_Retornar_Sigla_Da_Unidade()
        {
            var metro = new Comprimento(1.0, UnidadeComprimento.Metro);
            var cm = new Comprimento(100.0, UnidadeComprimento.Centimetro);
            var polegada = new Comprimento(12.0, UnidadeComprimento.Polegada);

            metro.ToString().Should().Be("1 m");
            cm.ToString().Should().Be("100 cm");
            polegada.ToString().Should().Be("12 in");
        }
    }
}