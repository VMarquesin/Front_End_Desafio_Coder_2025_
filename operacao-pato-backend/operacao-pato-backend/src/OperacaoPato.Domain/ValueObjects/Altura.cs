using System;

namespace OperacaoPato.Domain.ValueObjects
{
    public sealed class Altura
    {
        private const decimal CentimetrosPorPe = 30.48m;
        private const decimal CentimetrosPorJarda = 91.44m;

        public decimal Centimetros { get; }

        private Altura(decimal centimetros)
        {
            if (centimetros < 0)
                throw new ArgumentOutOfRangeException(nameof(centimetros), "Altura não pode ser negativa.");
            Centimetros = decimal.Round(centimetros, 4);
        }

        public static Altura FromCentimeters(decimal cm) => new Altura(cm);

        public static Altura FromFeet(decimal feet)
        {
            var cm = feet * CentimetrosPorPe;
            return new Altura(cm);
        }

        // Nova fábrica que aceita o enum UnidadeMedida do projeto
        public static Altura From(decimal value, UnidadeMedida unit)
        {
            if (object.Equals(unit, UnidadeMedida.Centimetro))
                return FromCentimeters(value);
            if (object.Equals(unit, UnidadeMedida.Metro))
                return FromCentimeters(value * 100);
            if (object.Equals(unit, UnidadeMedida.Pe))
                return FromFeet(value);
            if (object.Equals(unit, UnidadeMedida.Jarda))
                return new Altura(value * CentimetrosPorJarda);

            throw new ArgumentException($"Unidade de altura desconhecida: {unit}", nameof(unit));
        }

        public decimal ToCentimeters() => Centimetros;

        public decimal ToFeet() => decimal.Round(Centimetros / CentimetrosPorPe, 4);

        public static Altura From(decimal value, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unidade inválida.", nameof(unit));

            var u = unit.Trim().ToLowerInvariant();
            return u switch
            {
                "cm" or "centimeter" or "centimetro" or "centímetros" or "centimetros" => FromCentimeters(value),
                "m" or "metro" or "metros" => FromCentimeters(value * 100m),
                "ft" or "feet" or "pé" or "pe" or "pés" => FromFeet(value),
                "yd" or "yard" or "jarda" or "jardas" => new Altura(value * CentimetrosPorJarda),
                _ => throw new ArgumentException($"Unidade de altura desconhecida: {unit}", nameof(unit))
            };
        }

        public override string ToString() => $"{Centimetros} cm";

        public override bool Equals(object obj) => obj is Altura other && Centimetros == other.Centimetros;
        public override int GetHashCode() => Centimetros.GetHashCode();
    }
}