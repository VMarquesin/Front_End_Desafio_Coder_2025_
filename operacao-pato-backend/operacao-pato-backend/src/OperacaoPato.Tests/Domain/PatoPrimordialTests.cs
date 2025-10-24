using System;
using Xunit;
using OperacaoPato.Domain.Entities;
using OperacaoPato.Domain.Enums;

namespace OperacaoPato.Tests.Domain
{
    public class PatoPrimordialTests
    {
        [Fact]
        public void CriarPatoPrimordial_DeveCriarComValoresValidos()
        {
            // Arrange
            var pato = new PatoPrimordial
            {
                Altura = 100, // em cm
                Peso = 5000, // em g
                EstadoHibernacao = EstadoHibernacao.Desperto,
                QuantidadeMutacoes = 3,
                BatimentosCardiacos = 80 // em bpm
            };

            // Act & Assert
            Assert.Equal(100, pato.Altura);
            Assert.Equal(5000, pato.Peso);
            Assert.Equal(EstadoHibernacao.Desperto, pato.EstadoHibernacao);
            Assert.Equal(3, pato.QuantidadeMutacoes);
            Assert.Equal(80, pato.BatimentosCardiacos);
        }

        [Fact]
        public void AlterarEstadoHibernacao_DeveAlterarEstadoCorretamente()
        {
            // Arrange
            var pato = new PatoPrimordial
            {
                EstadoHibernacao = EstadoHibernacao.Desperto
            };

            // Act
            pato.EstadoHibernacao = EstadoHibernacao.Transe;

            // Assert
            Assert.Equal(EstadoHibernacao.Transe, pato.EstadoHibernacao);
        }

    }
}