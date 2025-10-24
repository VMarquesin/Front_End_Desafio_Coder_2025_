using System;
using OperacaoPato.Domain.Enums;
using OperacaoPato.Domain.ValueObjects;

namespace OperacaoPato.Domain.Entities
{
    public class PatoPrimordial
    {
        public Altura Altura { get; set; }
        public Peso Peso { get; set; }
        public EstadoHibernacao EstadoHibernacao { get; set; }
        public int? BatimentosCardiacos { get; set; }
        public int QuantidadeMutacoes { get; set; }
        public SuperPoder SuperPoder { get; set; }

        public PatoPrimordial(
            Altura altura,
            Peso peso,
            EstadoHibernacao estadoHibernacao,
            SuperPoder superPoder,
            int quantidadeMutacoes = 0,
            int? batimentosCardiacos = null)
        {
            Altura = altura ?? throw new ArgumentNullException(nameof(altura));
            Peso = peso ?? throw new ArgumentNullException(nameof(peso));
            SuperPoder = superPoder ?? throw new ArgumentNullException(nameof(superPoder));

            if (quantidadeMutacoes < 0)
                throw new ArgumentOutOfRangeException(nameof(quantidadeMutacoes), "Quantidade de mutações não pode ser negativa.");

            if (batimentosCardiacos.HasValue && batimentosCardiacos.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(batimentosCardiacos), "Batimentos cardíacos não pode ser negativo.");

            EstadoHibernacao = estadoHibernacao;
            QuantidadeMutacoes = quantidadeMutacoes;
            BatimentosCardiacos = batimentosCardiacos;
        }
    }
}