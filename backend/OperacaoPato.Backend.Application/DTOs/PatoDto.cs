namespace OperacaoPato.Backend.Application.DTOs
{
    public class PatoDto
    {
        public string DroneNumeroSerie { get; set; }

        // Altura
        public double AlturaValor { get; set; }
        public string AlturaUnidade { get; set; } = "Metro"; // exemplo: "Metro", "Centimetro"

        // Peso
        public double PesoValor { get; set; }
        public string PesoUnidade { get; set; } = "Quilograma"; // exemplo: "Grama", "Quilograma"

        // Localização
        public string Pais { get; set; } = default!;
        public string Cidade { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? PontoReferencia { get; set; } = default;

        public double Precisao { get; set; }
        public string PrecisaoUnidade { get; set; }

        // Estado fisiológico
        public string Status { get; set; } // StatusHibernacao como string ("Ativo", "Transe", "HibernacaoProfunda", etc.)
        public int? BatimentosPorMinuto { get; set; }

        // Mutação e poder
        public int QuantidadeMutacoes { get; set; }
        public string PoderNome { get; set; } = default!;   // Ajustar conforme SuperPoder
        public string PoderDescricao { get; set; }                // Ajustar conforme SuperPoder
        public string PoderClassificacao { get; set; }     
        // Data da coleta (opcional; default = UtcNow)
        public DateTime? DataColetaUtc { get; set; }
    }
}