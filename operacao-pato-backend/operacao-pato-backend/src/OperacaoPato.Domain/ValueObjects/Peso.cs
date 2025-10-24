using System;

namespace OperacaoPato.Domain.ValueObjects
{
    public sealed class Peso
    {
        private const decimal GramasPorLibra = 453.59237m;

        public decimal Gramas { get; }

        private Peso(decimal gramas)
        {
            if (gramas < 0)
                throw new ArgumentOutOfRangeException(nameof(gramas), "Peso não pode ser negativo.");
            Gramas = decimal.Round(gramas, 4);
        }
        public static Peso FromGrams(decimal grams) => new Peso(grams);

        public static Peso FromPounds(decimal pounds)
        {
            var g = pounds * GramasPorLibra;
            return new Peso(g);
        }

        public decimal ToGrams() => Gramas;

        public decimal ToPounds() => decimal.Round(Gramas / GramasPorLibra, 4);

        public static Peso From(decimal value, string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unidade inválida.", nameof(unit));

            var u = unit.Trim().ToLowerInvariant();
            return u switch
            {
                "g" or "gram" or "grama" or "gramas" => FromGrams(value),
                "lb" or "lbs" or "pound" or "libra" or "libras" => FromPounds(value),
                "kg" or "kilogram" or "quilograma" or "quilogramas" => FromGrams(value * 1000m),
                _ => throw new ArgumentException($"Unidade de peso desconhecida: {unit}", nameof(unit))
            };
        }
        public override string ToString() => $"{Gramas} g";
        public override bool Equals(object obj) => obj is Peso other && Gramas == other.Gramas;
        public override int GetHashCode() => Gramas.GetHashCode();
    }
}