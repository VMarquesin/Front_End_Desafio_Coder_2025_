namespace OperacaoPato.Backend.Application.DTOs
{
    public class DroneDto
    {
        public string NumeroSerie { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string PaisOrigem { get; set; } = string.Empty;
    }
}