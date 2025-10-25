using System;
using System.Globalization;

namespace OperacaoPato.Backend.Domain.ValueObjects
{
    public enum UnidadeMassa
    {
        Grama,
        Libra
    }

    public sealed class Massa
    {
        public double Valor { get; }
        public UnidadeMassa UnidadeMassa { get; }

        public Massa(double valor, UnidadeMassa unidade)
        {
            if (valor < 0) throw new ArgumentException("Massa não pode ser negativa.", nameof(valor));

            Valor = valor;
            UnidadeMassa = unidade;
        }

        private double ParaGramas()
        {
            return UnidadeMassa switch
            {
                UnidadeMassa.Grama => Valor,
                UnidadeMassa.Libra => Valor * 453.59237,
                _ => throw new NotSupportedException($"Unidade '{UnidadeMassa}' não suportada.")
            };
        }

        private static Massa DeGramas(double gramas, UnidadeMassa unidadeAlvo)
        {
            double convertido = unidadeAlvo switch
            {
                UnidadeMassa.Grama => gramas,
                UnidadeMassa.Libra => gramas / 453.59237,
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
        public double EmLibras() => ParaGramas() / 453.59237;
        
        public double Em(UnidadeMassa unidadeAlvo) => ConverterPara(unidadeAlvo).Valor;

        private static string Sigla(UnidadeMassa unidade) =>
            unidade switch
            {
                UnidadeMassa.Grama => "g",
                UnidadeMassa.Libra => "lb",
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