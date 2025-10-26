using System;

namespace OperacaoPato.Backend.Domain.ValueObjects;

public class NivelRecurso
{
    public double Valor { get; private set; }
    public double ValorMaximo { get; private set; }
    public string Unidade { get; private set; }

    public NivelRecurso(double valor, double valorMaximo, string unidade)
    {
        if (valor < 0 || valorMaximo <= 0)
            throw new ArgumentException("Valores devem ser positivos");
        if (valor > valorMaximo)
            throw new ArgumentException("Valor atual não pode exceder máximo");
        if (string.IsNullOrWhiteSpace(unidade))
            throw new ArgumentException("Unidade é obrigatória");

        Valor = valor;
        ValorMaximo = valorMaximo;
        Unidade = unidade;
    }

    public double PorcentagemAtual => (Valor / ValorMaximo) * 100;

    public bool EmNivelCritico => PorcentagemAtual <= 20;

    public void Consumir(double quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentException("Quantidade deve ser positiva");
        if (quantidade > Valor)
            throw new InvalidOperationException("Recurso insuficiente");

        Valor -= quantidade;
    }

    public void Reabastecer(double quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentException("Quantidade deve ser positiva");
        
        Valor = Math.Min(Valor + quantidade, ValorMaximo);
    }

    private NivelRecurso() 
    { 
        Unidade = ""; 
        ValorMaximo = 0;
        Valor = 0;
    } // EF
}