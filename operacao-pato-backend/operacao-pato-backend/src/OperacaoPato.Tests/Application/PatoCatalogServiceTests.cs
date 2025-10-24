using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OperacaoPato.Application.Services;
using OperacaoPato.Application.Interfaces;
using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Tests.Application
{
    [TestFixture]
    public class PatoCatalogServiceTests
    {
        private PatoCatalogService _patoCatalogService;
        private Mock<IPatoRepository> _patoRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _patoRepositoryMock = new Mock<IPatoRepository>();
            _patoCatalogService = new PatoCatalogService(_patoRepositoryMock.Object);
        }

        [Test]
        public async Task AddPatoPrimordial_ShouldCallRepositoryAdd()
        {
            // Arrange
            var pato = new PatoPrimordial
            {
                Altura = 100,
                Peso = 5000,
                EstadoHibernacao = Enums.EstadoHibernacao.Desperto,
                QuantidadeMutacoes = 2,
                SuperPoder = new SuperPoder { Nome = "Tempestade Elétrica", Descricao = "Gera descargas elétricas em área", Classificacao = "Bélico" }
            };

            // Act
            await _patoCatalogService.AddPatoPrimordial(pato);

            // Assert
            _patoRepositoryMock.Verify(repo => repo.AddAsync(pato), Times.Once);
        }

        [Test]
        public async Task GetAllPatosPrimordiais_ShouldReturnListOfPatos()
        {
            // Arrange
            var patos = new List<PatoPrimordial>
            {
                new PatoPrimordial { Altura = 100, Peso = 5000 },
                new PatoPrimordial { Altura = 90, Peso = 4500 }
            };

            _patoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patos);

            // Act
            var result = await _patoCatalogService.GetAllPatosPrimordiais();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task UpdatePatoPrimordial_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var pato = new PatoPrimordial
            {
                Altura = 100,
                Peso = 5000,
                EstadoHibernacao = Enums.EstadoHibernacao.Desperto
            };

            // Act
            await _patoCatalogService.UpdatePatoPrimordial(pato);

            // Assert
            _patoRepositoryMock.Verify(repo => repo.UpdateAsync(pato), Times.Once);
        }

        [Test]
        public async Task DeletePatoPrimordial_ShouldCallRepositoryDelete()
        {
            // Arrange
            var patoId = Guid.NewGuid();

            // Act
            await _patoCatalogService.DeletePatoPrimordial(patoId);

            // Assert
            _patoRepositoryMock.Verify(repo => repo.DeleteAsync(patoId), Times.Once);
        }
    }
}