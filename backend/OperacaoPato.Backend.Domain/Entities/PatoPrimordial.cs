using System;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Backend.Domain.Enums;
using OperacaoPato.Domain.ValueObjects;
using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Backend.Domain.Entities
{
    public sealed class PatoPrimordial
    {
        public Guid Id { get; }
        public string DroneNumeroSerie { get; }
        public Comprimento Altura { get; }
        public Massa Peso { get; }
        public Localizacao Localizacao { get; }
        public StatusHibernacao Status { get; }
        public int? BatimentosPorMinuto { get; }
        public int QuantidadeMutacoes { get; }
        public SuperPoder Poder { get; }
        public DateTime DataColetaUtc { get; }

        public PatoPrimordial(
            string droneNumeroSerie,
            Comprimento altura,
            Massa peso,
            Localizacao localizacao,
            StatusHibernacao status,
            int? batimentosPorMinuto,
            int quantidadeMutacoes,
            SuperPoder poder,
            DateTime? dataColetaUtc = null)
            : this(drone: null,
                   droneNumeroSerie: droneNumeroSerie,
                   altura: altura,
                   peso: peso,
                   localizacao: localizacao,
                   status: status,
                   batimentosPorMinuto: batimentosPorMinuto,
                   quantidadeMutacoes: quantidadeMutacoes,
                   poder: poder,
                   dataColetaUtc: dataColetaUtc)
        { }

        public PatoPrimordial(
            Drone? drone,
            string? droneNumeroSerie,
            Comprimento altura,
            Massa peso,
            Localizacao localizacao,
            StatusHibernacao status,
            int? batimentosPorMinuto,
            int quantidadeMutacoes,
            SuperPoder poder,
            DateTime? dataColetaUtc = null)
        {
            Id = Guid.NewGuid();
            DroneNumeroSerie = drone?.NumeroSerie ?? droneNumeroSerie ?? string.Empty;

            Altura = altura;
            if (Altura.Valor <= 0) throw new ArgumentException("Altura deve ser maior que zero.", nameof(altura));

            Peso = peso;
            if (Peso.EmGramas() <= 0) throw new ArgumentException("Peso deve ser maior que zero.", nameof(peso));

            Localizacao = localizacao;
            Status = status;

            if (status == StatusHibernacao.Transe || status == StatusHibernacao.HibernacaoProfunda)
            {
                if (!batimentosPorMinuto.HasValue)
                    throw new ArgumentException("Batimentos cardíacos são obrigatórios para transe/hibernação profunda.", nameof(batimentosPorMinuto));
                if (batimentosPorMinuto <= 0)
                    throw new ArgumentException("Batimentos cardíacos devem ser positivos.", nameof(batimentosPorMinuto));
                BatimentosPorMinuto = batimentosPorMinuto;
            }
            else
            {
                if (batimentosPorMinuto.HasValue && batimentosPorMinuto <= 0)
                    throw new ArgumentException("Batimentos cardíacos devem ser positivos.", nameof(batimentosPorMinuto));
                BatimentosPorMinuto = batimentosPorMinuto;
            }

            if (quantidadeMutacoes < 0) throw new ArgumentException("Quantidade de mutações não pode ser negativa.", nameof(quantidadeMutacoes));
            QuantidadeMutacoes = quantidadeMutacoes;

            Poder = poder;
            DataColetaUtc = dataColetaUtc?.ToUniversalTime() ?? DateTime.UtcNow;
        }

        public Comprimento ObterAlturaEm(UnidadeComprimento unidade) => Altura.ConverterPara(unidade);
        public string MostrarAltura()
        {
            return Altura.ToString();
        }

        public double ObterPesoEm(UnidadeMassa unidade) => Peso.Em(unidade);
        public string MostrarPeso(bool usarUnidadeOriginal = true, UnidadeMassa? unidade = null, int casasDecimais = 2)
        {
            if (usarUnidadeOriginal || unidade == null)
                return Peso.ToString();
            return Peso.ToString(unidade.Value, casasDecimais);
        }

        public override string ToString()
        {
            var droneInfo = string.IsNullOrEmpty(DroneNumeroSerie) ? "<sem-drone>" : DroneNumeroSerie;
            return $"{Id} - Drone:{droneInfo} - {Localizacao.Cidade}/{Localizacao.Pais} - Status:{Status} - Altura:{Altura} - Peso:{Peso.ToString()}";
        }
    }
}