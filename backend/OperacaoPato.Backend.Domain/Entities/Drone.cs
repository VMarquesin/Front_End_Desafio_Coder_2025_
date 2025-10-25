using System;

namespace OperacaoPato.Domain.Entities
{
    public class Drone
    {
        public string NumeroSerie { get; private set; }
        public string Marca { get; private set; }
        public string Fabricante { get; private set; }
        public string PaisOrigem { get; private set; }

        public Drone(string numeroSerie, string marca, string fabricante, string paisOrigem)
        {
            NumeroSerie = numeroSerie;
            Marca = marca;
            Fabricante = fabricante;
            PaisOrigem = paisOrigem;
        }
    }
}