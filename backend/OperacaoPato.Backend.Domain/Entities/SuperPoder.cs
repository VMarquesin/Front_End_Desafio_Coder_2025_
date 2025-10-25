namespace OperacaoPato.Backend.Domain.Entities
{
    public sealed class SuperPoder
    {
        public string Nome { get; }
        public string Descricao { get; }
        // Classificação agora livre, definida pelo usuário
        public string Classificacao { get; }

        public SuperPoder(string nome, string descricao, string classificacao)
        {
            Nome = nome;
            Descricao = descricao;
            Classificacao = classificacao;
        }

        public override string ToString() =>
            $"{Nome} — {Classificacao}: {Descricao}";
    }
}