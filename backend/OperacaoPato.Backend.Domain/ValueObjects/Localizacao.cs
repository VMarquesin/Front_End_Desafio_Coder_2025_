using System;
using OperacaoPato.Domain.ValueObjects; 

namespace OperacaoPato.Backend.Domain.ValueObjects
{
    public sealed class Localizacao
    {
        public string Cidade { get; }
        public string Pais { get; }
        public Coordenada Coordenada { get; }
        public Comprimento Precisao { get; }
        public string? PontoReferencia { get; }

        public Localizacao(string cidade, string pais, Coordenada coordenada, Comprimento precisao, string? pontoReferencia = null)
        {

            var precisaoEmMetros = precisao.ConverterPara(UnidadeComprimento.Metro).Valor;

            // 4 cm = 0.04 m
            if (precisaoEmMetros < 0.04)
                throw new ArgumentException("Precisão menor que 4 cm não é válida.");

            if (precisaoEmMetros > 30.0)
                throw new ArgumentException("Precisão maior que 30 m não é válida.");

            Cidade = cidade;
            Pais = pais;
            Coordenada = coordenada;
            Precisao = precisao;
            PontoReferencia = string.IsNullOrWhiteSpace(pontoReferencia) ? null : pontoReferencia;
        }
    }
}