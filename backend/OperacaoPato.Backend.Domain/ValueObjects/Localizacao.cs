using System;
using OperacaoPato.Backend.Domain.Enums; 

namespace OperacaoPato.Backend.Domain.ValueObjects
{
    public sealed class Localizacao
    {
        public string Cidade { get; private set; }
        public string Pais { get; private set; }
        public Coordenada Coordenada { get; private set; }
        public Comprimento Precisao { get; private set; }
        public string? PontoReferencia { get; private set; }

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

        // Parameterless ctor para EF
        private Localizacao()
        {
            Cidade = string.Empty;
            Pais = string.Empty;
            Coordenada = new Coordenada(0.0, 0.0);
            Precisao = new Comprimento(0.0, UnidadeComprimento.Metro);
            PontoReferencia = null;
        }
    }
}