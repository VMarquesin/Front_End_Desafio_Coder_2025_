using System;

namespace OperacaoPato.Backend.Domain.Entities
{
    public abstract class Drone
    {
        public Guid Id { get; protected set; }
        public string NumeroSerie { get; protected set; } = "";
        public string Marca { get; protected set; } = "";
        public string Fabricante { get; protected set; } = "";
        public string PaisOrigem { get; protected set; } = "";

        protected Drone(string numeroSerie, string marca, string fabricante, string paisOrigem)
        {
            Id = Guid.NewGuid();
            NumeroSerie = numeroSerie ?? throw new ArgumentNullException(nameof(numeroSerie));
            Marca = marca ?? throw new ArgumentNullException(nameof(marca));
            Fabricante = fabricante ?? throw new ArgumentNullException(nameof(fabricante));
            PaisOrigem = paisOrigem ?? throw new ArgumentNullException(nameof(paisOrigem));

            ValidarPropriedades();
        }

        protected Drone() 
        { 
            Id = Guid.NewGuid();
        } // EF Core

        private void ValidarPropriedades()
        {
            if (string.IsNullOrWhiteSpace(NumeroSerie))
                throw new ArgumentException("Número de série não pode estar vazio", nameof(NumeroSerie));
            if (string.IsNullOrWhiteSpace(Marca))
                throw new ArgumentException("Marca não pode estar vazia", nameof(Marca));
            if (string.IsNullOrWhiteSpace(Fabricante))
                throw new ArgumentException("Fabricante não pode estar vazio", nameof(Fabricante));
            if (string.IsNullOrWhiteSpace(PaisOrigem))
                throw new ArgumentException("País de origem não pode estar vazio", nameof(PaisOrigem));
        }
    }
}