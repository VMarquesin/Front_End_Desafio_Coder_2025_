using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Application.UseCases.ObterTodosPatos
{
    public class ObterTodosPatosUseCase : IObterTodosPatosUseCase
    {
        private readonly IPatoRepository _repo;

        public ObterTodosPatosUseCase(IPatoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PatoDto>> HandleAsync(CancellationToken ct = default)
        {
            var list = await _repo.ObterTodosAsync();

            return list.Select(MapToDto);
        }

        private static PatoDto MapToDto(PatoPrimordial created)
        {
            return new PatoDto
            {
                Id = created.Id,
                DroneNumeroSerie = created.DroneNumeroSerie,
                AlturaValor = created.Altura.Valor,
                AlturaUnidade = created.Altura.UnidadeComprimento.ToString(),
                PesoValor = created.Peso.Em(created.Peso.UnidadeMassa),
                PesoUnidade = created.Peso.UnidadeMassa.ToString(),
                Pais = created.Localizacao.Pais,
                Cidade = created.Localizacao.Cidade,
                Latitude = created.Localizacao.Coordenada.Latitude,
                Longitude = created.Localizacao.Coordenada.Longitude,
                Status = created.Status.ToString(),
                BatimentosPorMinuto = created.BatimentosPorMinuto,
                QuantidadeMutacoes = created.QuantidadeMutacoes,
                PoderNome = created.Poder.Nome,
                PoderDescricao = created.Poder.Descricao,
                PoderClassificacao = created.Poder.Classificacao,
                DataColetaUtc = created.DataColetaUtc,
                Precisao = created.Localizacao.Precisao.Valor,
                PrecisaoUnidade = created.Localizacao.Precisao.UnidadeComprimento.ToString(),
            };
        }
    }
}