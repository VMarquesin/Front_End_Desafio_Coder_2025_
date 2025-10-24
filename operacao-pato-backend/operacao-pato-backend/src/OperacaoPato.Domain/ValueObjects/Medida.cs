namespace OperacaoPato.Domain.ValueObjects
{
    public class Medida
    {
        public decimal Valor { get; private set; }
        public UnidadeMedida Unidade { get; private set; }

        public Medida(decimal valor, UnidadeMedida unidade)
        {
            Valor = valor;
            Unidade = unidade;
        }

        public decimal ConverterPara(UnidadeMedida novaUnidade)
        {
            if (Unidade == novaUnidade)
            {
                return Valor;
            }

            // Implementar lógica de conversão entre unidades
            // Exemplo: se a unidade atual for metros e a nova for pés
            if (Unidade == UnidadeMedida.Metros && novaUnidade == UnidadeMedida.Pes)
            {
                return Valor * 3.28084m; // Conversão de metros para pés
            }
            else if (Unidade == UnidadeMedida.Pes && novaUnidade == UnidadeMedida.Metros)
            {
                return Valor / 3.28084m; // Conversão de pés para metros
            }

            // Adicionar outras conversões conforme necessário

            throw new InvalidOperationException("Conversão não suportada entre as unidades fornecidas.");
        }
    }
}