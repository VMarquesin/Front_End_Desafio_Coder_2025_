using System;

namespace OperacaoPato.Domain.ValueObjects
{
    public enum UnidadeComprimento
    {
        Metro,
        Centimetro,
        Quilometro,
        Polegada,
        Pe,
        Jarda
    }
    public sealed class Comprimento
    {
        public double Valor { get; }
        public UnidadeComprimento UnidadeComprimento { get; }

        public Comprimento(double valor, UnidadeComprimento unidade)
        {
            if (valor < 0)
                throw new ArgumentException("A distância não pode ser negativa.");

            Valor = valor;
            UnidadeComprimento = unidade;
        }

        private double ParaMetros()
        {
            return UnidadeComprimento switch
            {
                UnidadeComprimento.Metro => Valor,
                UnidadeComprimento.Centimetro => Valor / 100.0,
                UnidadeComprimento.Quilometro => Valor * 1000.0,
                UnidadeComprimento.Polegada => Valor * 0.0254,
                UnidadeComprimento.Pe => Valor * 0.3048,
                UnidadeComprimento.Jarda => Valor * 0.9144,
                _ => throw new NotSupportedException($"UnidadeComprimento '{UnidadeComprimento}' não suportada.")
            };
        }

        private static Comprimento DeMetros(double metros, UnidadeComprimento unidadeAlvo)
        {
            double convertido = unidadeAlvo switch
            {
                UnidadeComprimento.Metro => metros,
                UnidadeComprimento.Centimetro => metros * 100.0,
                UnidadeComprimento.Quilometro => metros / 1000.0,
                UnidadeComprimento.Polegada => metros / 0.0254,
                UnidadeComprimento.Pe => metros / 0.3048,
                UnidadeComprimento.Jarda => metros / 0.9144,
                _ => throw new NotSupportedException($"UnidadeComprimento '{unidadeAlvo}' não suportada.")
            };

            return new Comprimento(convertido, unidadeAlvo);
        }

        public Comprimento ConverterPara(UnidadeComprimento unidadeAlvo)
        {
            var metros = ParaMetros();
            return DeMetros(metros, unidadeAlvo);
        }

        public Comprimento Somar(Comprimento outra)
        {
            double totalMetros = ParaMetros() + outra.ParaMetros();
            return DeMetros(totalMetros, UnidadeComprimento);
        }

        public Comprimento Subtrair(Comprimento outra)
        {
            double resultadoMetros = ParaMetros() - outra.ParaMetros();
            if (resultadoMetros < 0)
                throw new InvalidOperationException("O resultado não pode ser negativo.");

            return DeMetros(resultadoMetros, UnidadeComprimento);
        }

        public bool Equals(Comprimento? outra)
        {
            if (outra is null) return false;
            return Math.Abs(ParaMetros() - outra.ParaMetros()) < 1e-9;
        }

        public override bool Equals(object? obj) => Equals(obj as Comprimento);
        public override int GetHashCode() => ParaMetros().GetHashCode();

        // Método auxiliar para retornar a sigla da unidade
        private string Sigla()
        {
            return UnidadeComprimento switch
            {
                UnidadeComprimento.Metro => "m",
                UnidadeComprimento.Centimetro => "cm",
                UnidadeComprimento.Quilometro => "km",
                UnidadeComprimento.Polegada => "in",
                UnidadeComprimento.Pe => "ft",
                UnidadeComprimento.Jarda => "yd",
                _ => UnidadeComprimento.ToString()
            };
        }

        // Agora retorna "valor sigla", ex: "1 m"
        public override string ToString() => $"{Valor} {Sigla()}";
    }
}