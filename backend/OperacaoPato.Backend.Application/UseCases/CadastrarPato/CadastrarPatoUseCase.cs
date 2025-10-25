using FluentValidation;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Shared.Exceptions;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Backend.Domain.Enums;
using OperacaoPato.Domain.ValueObjects;

namespace OperacaoPato.Backend.Application.UseCases.CadastrarPato
{
    public class CadastrarPatoUseCase : ICadastrarPatoUseCase
    {
        private readonly IPatoRepository _repo;
        private readonly IValidator<PatoDto> _validator;

        public CadastrarPatoUseCase(IPatoRepository repo, IValidator<PatoDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<PatoDto> HandleAsync(PatoDto input, CancellationToken ct = default)
        {
            var result = await _validator.ValidateAsync(input, ct);
            if (!result.IsValid)
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());

            var dataColeta = (input.DataColetaUtc ?? DateTime.UtcNow).ToUniversalTime();
            if (await _repo.ExisteAsync(input.DroneNumeroSerie, dataColeta))
                throw new ErrorOnValidationException($"Já existe Pato para drone {input.DroneNumeroSerie} na data {dataColeta:o}.");

            try
            {
                // Converter siglas -> enums (com fallback para nome do enum)
                var unidadeComprimento = ParseUnidadeComprimento(input.AlturaUnidade);
                var altura = new Comprimento(input.AlturaValor, unidadeComprimento);

                var unidadeMassa = ParseUnidadeMassa(input.PesoUnidade);
                var peso = new Massa(input.PesoValor, unidadeMassa);

                var coordenada = new Coordenada(input.Latitude, input.Longitude);
                var unidadeComprimentoPrecisao = ParseUnidadeComprimento(input.PrecisaoUnidade);
                var precisao = new Comprimento(input.Precisao, unidadeComprimentoPrecisao);
                var localizacao = new Localizacao(input.Pais, input.Cidade, coordenada, precisao, input.PontoReferencia);

                var status = Enum.Parse<StatusHibernacao>(input.Status, ignoreCase: true);

                var poder = new SuperPoder(input.PoderNome, input.PoderDescricao, input.PoderClassificacao);

                var entity = new PatoPrimordial(
                    droneNumeroSerie: input.DroneNumeroSerie,
                    altura: altura,
                    peso: peso,
                    localizacao: localizacao,
                    status: status,
                    batimentosPorMinuto: input.BatimentosPorMinuto,
                    quantidadeMutacoes: input.QuantidadeMutacoes,
                    poder: poder,
                    dataColetaUtc: dataColeta
                );

                var created = await _repo.AdicionarAsync(entity);

                return new PatoDto
                {
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
                    DataColetaUtc = created.DataColetaUtc
                };
            }
            catch (ErrorOnValidationException) { throw; }
            catch (Exception ex)
            {
                throw new ErrorOnValidationException(new List<string> { ex.Message });
            }
        }

        // Mapeia siglas conhecidas; se não achar, tenta o nome do enum; senão lança validação
        private static UnidadeComprimento ParseUnidadeComprimento(string value)
        {
            var map = new Dictionary<string, UnidadeComprimento>(StringComparer.OrdinalIgnoreCase)
            {
                ["m"] = UnidadeComprimento.Metro,
                ["cm"] = UnidadeComprimento.Centimetro,
                ["km"] = UnidadeComprimento.Quilometro
            };

            if (map.TryGetValue(value, out var unidade))
                return unidade;

            if (Enum.TryParse<UnidadeComprimento>(value, true, out var unidadeByName))
                return unidadeByName;

            throw new ErrorOnValidationException(new List<string> {
                $"Unidade de comprimento '{value}' inválida. Use siglas: {string.Join(", ", map.Keys)} ou nomes: {string.Join(", ", Enum.GetNames(typeof(UnidadeComprimento)))}."
            });
        }

        private static UnidadeMassa ParseUnidadeMassa(string value)
        {
            var map = new Dictionary<string, UnidadeMassa>(StringComparer.OrdinalIgnoreCase)
            {
                ["g"]  = UnidadeMassa.Grama,
                ["lb"] = UnidadeMassa.Libra
                // se o domínio tiver 'Libra' adicione: ["lb"] = UnidadeMassa.Libra
            };

            if (map.TryGetValue(value, out var unidade))
                return unidade;

            if (Enum.TryParse<UnidadeMassa>(value, true, out var unidadeByName))
                return unidadeByName;

            throw new ErrorOnValidationException(new List<string> {
                $"PesoUnidade '{value}' inválida. Use siglas: {string.Join(", ", map.Keys)} ou nomes: {string.Join(", ", Enum.GetNames(typeof(UnidadeMassa)))}."
            });
        }
    }
}