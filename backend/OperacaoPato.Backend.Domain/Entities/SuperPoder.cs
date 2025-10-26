namespace OperacaoPato.Backend.Domain.Entities
{
    public sealed class SuperPoder
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        // Classificação agora livre, definida pelo usuário
        public string Classificacao { get; private set; }

        public SuperPoder(string nome, string descricao, string classificacao)
        {
            Nome = nome;
            Descricao = descricao;
            Classificacao = classificacao;
        }

        // Parameterless ctor para EF
        private SuperPoder()
        {
            Nome = string.Empty;
            Descricao = string.Empty;
            Classificacao = string.Empty;
        }

        public override string ToString() =>
            $"{Nome} — {Classificacao}: {Descricao}";
    }
}