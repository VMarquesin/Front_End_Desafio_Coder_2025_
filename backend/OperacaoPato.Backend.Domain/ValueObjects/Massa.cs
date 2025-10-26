using System;
using System.Globalization;
using OperacaoPato.Backend.Domain.Enums;

namespace OperacaoPato.Backend.Domain.ValueObjects
{


    public sealed class Massa
    {
        public double Valor { get; private set; }
        public UnidadeMassa UnidadeMassa { get; private set; }

        public Massa(double valor, UnidadeMassa unidade)
        {
            if (valor < 0) throw new ArgumentException("Massa não pode ser negativa.", nameof(valor));

            Valor = valor;
            UnidadeMassa = unidade;
        }

        // Parameterless ctor para EF Core
        private Massa()
        {
            Valor = 0.0;
            UnidadeMassa = UnidadeMassa.Grama;
        }

        private double ParaGramas()
        {
            return UnidadeMassa switch
            {
                UnidadeMassa.Miligrama => Valor / 1000,
                UnidadeMassa.Grama => Valor,
                UnidadeMassa.Quilograma => Valor * 1000,
                UnidadeMassa.Tonelada => Valor * 1_000_000,
                _ => throw new NotSupportedException($"Unidade '{UnidadeMassa}' não suportada.")
            };
        }

        private static Massa DeGramas(double gramas, UnidadeMassa unidadeAlvo)
        {
            double convertido = unidadeAlvo switch
            {
                UnidadeMassa.Miligrama => gramas * 1000,
                UnidadeMassa.Grama => gramas,
                UnidadeMassa.Quilograma => gramas / 1000,
                UnidadeMassa.Tonelada => gramas / 1_000_000,
                _ => throw new NotSupportedException($"Unidade '{unidadeAlvo}' não suportada.")
            };

            return new Massa(convertido, unidadeAlvo);
        }

        public Massa ConverterPara(UnidadeMassa unidadeAlvo)
        {
            var gramas = ParaGramas();
            return DeGramas(gramas, unidadeAlvo);
        }

        public double EmGramas() => ParaGramas();
        public double Em(UnidadeMassa unidadeAlvo) => ConverterPara(unidadeAlvo).Valor;

        private static string Sigla(UnidadeMassa unidade) =>
            unidade switch
            {
                UnidadeMassa.Miligrama => "mg",
                UnidadeMassa.Grama => "g",
                UnidadeMassa.Quilograma => "kg",
                UnidadeMassa.Tonelada => "t",
                _ => unidade.ToString()
            };

        public string ToString(UnidadeMassa unidadeAlvo, int casasDecimais = 2)
        {
            var convertido = ConverterPara(unidadeAlvo);
            string formato = "F" + Math.Max(0, casasDecimais);
            return $"{convertido.Valor.ToString(formato, CultureInfo.InvariantCulture)} {Sigla(unidadeAlvo)}";
        }

        public override string ToString() => $"{Valor} {Sigla(UnidadeMassa)}";

        public override bool Equals(object? obj)
        {
            if (obj is not Massa outra) return false;
            return Math.Abs(ParaGramas() - outra.ParaGramas()) < 1e-9;
        }

        public override int GetHashCode() => ParaGramas().GetHashCode();
    }
}