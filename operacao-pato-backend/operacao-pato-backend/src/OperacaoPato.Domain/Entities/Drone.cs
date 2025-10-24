using System;

namespace OperacaoPato.Domain.Entities
{
    public class Drone
    {
        public string NumeroSerie { get; set; }
        public string Marca { get; set; }
        public string Fabricante { get; set; }
        public string PaisOrigem { get; set; }
        public int PrecisaoLocalizacao { get; set; }
    }
}