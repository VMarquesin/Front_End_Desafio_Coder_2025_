using System;

namespace OperacaoPato.Application.DTOs
{
    public class PatoDto
    {
        public string NumeroSerieDrone { get; set; }
        public string MarcaDrone { get; set; }
        public string FabricanteDrone { get; set; }
        public string PaisOrigemDrone { get; set; }
        public decimal Altura { get; set; } 
        public string UnidadeAltura { get; set; }
        public decimal Peso { get; set; }
        public string UnidadePeso {get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Precisao { get; set; }
        public string PontoReferencia { get; set; }
        public string EstadoHibernacao { get; set; } 
        public int? BatimentosCardiacos { get; set; }
        public int QuantidadeMutacoes { get; set; }
        public string NomeSuperPoder { get; set; }
        public string DescricaoSuperPoder { get; set; }
        public string ClassificacaoSuperPoder { get; set; }
    }
}