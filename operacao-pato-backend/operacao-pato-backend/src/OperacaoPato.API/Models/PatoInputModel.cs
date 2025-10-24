using System.ComponentModel.DataAnnotations;
using System;
using OperacaoPato.Application.DTOs;

namespace OperacaoPato.API.Models
{
    public class PatoInputModel
    {
        [Required]
        public string NumeroSerieDrone { get; set; }

        [Required]
        public string MarcaDrone { get; set; }

        [Required]
        public string FabricanteDrone { get; set; }

        [Required]
        public string PaisOrigemDrone { get; set; }

        [Required]
        [Range(0, 300)]
        public double Altura { get; set; } // em cm

        [Required]
        [Range(0, 100000)]
        public double Peso { get; set; } // em g

        [Required]
        public string UnidadeAltura { get; set; } // cm ou pés

        [Required]
        public string UnidadePeso { get; set; } // g ou libras

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string Pais { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Precisao { get; set; } // em cm ou jardas

        [Required]
        public string UnidadePrecisao { get; set; } // cm ou jardas

        public string PontoReferencia { get; set; }

        [Required]
        public string EstadoHibernacao { get; set; } // Desperto, Transe, HibernacaoProfunda

        public int? BatimentosCardiacos { get; set; } // bpm, opcional

        [Required]
        public int QuantidadeMutacoes { get; set; }

        public string SuperPoderNome { get; set; }
        public string SuperPoderDescricao { get; set; }
        public string SuperPoderClassificacao { get; set; }

        public PatoDto ToDto()
        {
            return new PatoDto
            {
                NumeroSerieDrone = this.NumeroSerieDrone,
                MarcaDrone = this.MarcaDrone,
                FabricanteDrone = this.FabricanteDrone,
                PaisOrigemDrone = this.PaisOrigemDrone,
                Altura = this.Altura,
                Peso = this.Peso,
                Localizacao = new Application.DTOs.LocalizacaoDto
                {
                    Cidade = this.Cidade,
                    Pais = this.Pais,
                    Latitude = this.Latitude,
                    Longitude = this.Longitude,
                    Precisao = this.Precisao,
                    PontoReferencia = this.PontoReferencia
                },
                EstadoHibernacao = this.EstadoHibernacao,
                BatimentosCardiacos = this.BatimentosCardiacos,
                QuantidadeMutacoes = this.QuantidadeMutacoes,
                SuperPoderNome = this.SuperPoderNome,
                SuperPoderDescricao = this.SuperPoderDescricao,
                SuperPoderClassificacao = this.SuperPoderClassificacao
            };
        }
    }

    public class SuperPoderInputModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Classificacao { get; set; } // Ex: bélico, raro, alto risco de curto-circuito
    }
}