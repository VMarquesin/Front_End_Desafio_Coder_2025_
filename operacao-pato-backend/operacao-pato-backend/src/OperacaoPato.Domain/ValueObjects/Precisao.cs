using System;

namespace OperacaoPato.Domain.ValueObjects
{
    public class Precisao
    {
        public double Valor { get; private set; }
        public UnidadeMedida Unidade { get; private set; }

        public Precisao(double valor, UnidadeMedida unidade)
        {
            if(valor < 4 || valor > 30)
                throw new ArgumentException(nameof(valor), "Precisão deve estar entre 4 e 30.");
            Valor = valor;
            Unidade = unidade;
        }

        public double ConverterParaMetros()
        {
            if (Unidade == UnidadeMedida.Metro)
                return Valor;
            if (Unidade == UnidadeMedida.Centimetro)
                return Valor / 100;
            if (Unidade == UnidadeMedida.Jarda)
                return Valor * 0.9144;
            if (Unidade == UnidadeMedida.Pe)
                return Valor * 0.3048;

            throw new ArgumentException("Unidade de medida não suportada.");
        }
    }
}