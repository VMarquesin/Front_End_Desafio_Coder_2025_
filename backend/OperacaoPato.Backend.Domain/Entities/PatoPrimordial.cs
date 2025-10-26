using System;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Backend.Domain.Enums;

namespace OperacaoPato.Backend.Domain.Entities
{
    public sealed class PatoPrimordial
    {
        public Guid Id { get; private set; }
        public string DroneNumeroSerie { get; private set; }
        public Comprimento Altura { get; private set; }
        public Massa Peso { get; private set; }
        public Localizacao Localizacao { get; private set; }
        public StatusHibernacao Status { get; private set; }
        public int? BatimentosPorMinuto { get; private set; }
        public int QuantidadeMutacoes { get; private set; }
        public SuperPoder Poder { get; private set; }
        public DateTime DataColetaUtc { get; private set; }

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

    // Parameterless constructor for EF Core
        private PatoPrimordial()
        {
            // Inicializadores padrão para satisfazer o compilador e permitir instanciação pelo EF
            Id = Guid.Empty;
            DroneNumeroSerie = string.Empty;
            Altura = default!;
            Peso = default!;
            Localizacao = default!;
            Status = default;
            BatimentosPorMinuto = null;
            QuantidadeMutacoes = 0;
            Poder = default!;
            DataColetaUtc = DateTime.UtcNow;
        }

        public Comprimento ObterAlturaEm(UnidadeComprimento unidade) => Altura.ConverterPara(unidade);
        public string MostrarAltura()
        {
            return Altura.ToString();
        }

        public double ObterPesoEm(Enums.UnidadeMassa unidade) => Peso.Em(unidade);
        public string MostrarPeso(bool usarUnidadeOriginal = true, Enums.UnidadeMassa? unidade = null, int casasDecimais = 2)
        {
            if (usarUnidadeOriginal || unidade is null)
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