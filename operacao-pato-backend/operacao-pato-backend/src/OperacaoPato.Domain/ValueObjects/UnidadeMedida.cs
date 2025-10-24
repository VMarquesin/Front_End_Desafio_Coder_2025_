namespace OperacaoPato.Domain.ValueObjects
{
    public class UnidadeMedida
    {
        public string Nome { get; private set; }
        public string Simbolo { get; private set; }

        public UnidadeMedida(string nome, string simbolo)
        {
            Nome = nome;
            Simbolo = simbolo;
        }

        public static UnidadeMedida Centimetro => new UnidadeMedida("Centímetro", "cm");
        public static UnidadeMedida Metro => new UnidadeMedida("Metro", "m");
        public static UnidadeMedida Pe => new UnidadeMedida("Pé", "ft");
        public static UnidadeMedida Libra => new UnidadeMedida("Libra", "lb");
        public static UnidadeMedida Grama => new UnidadeMedida("Grama", "g");
        public static UnidadeMedida Jarda => new UnidadeMedida("Jarda", "yd");
    }
}