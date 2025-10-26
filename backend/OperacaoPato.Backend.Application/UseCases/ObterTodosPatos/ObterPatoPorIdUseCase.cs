using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Interfaces; 
using System;
using System.Threading.Tasks;
using OperacaoPato.Backend.Domain.Services;
using OperacaoPato.Backend.Application.Services;
// --- IMPORTANTE: Precisamos importar a entidade PatoPrimordial ---
using OperacaoPato.Backend.Domain.Entities; 

namespace OperacaoPato.Backend.Application.UseCases.ObterPatoPorId 
{
    public class ObterPatoPorIdUseCase : IObterPatoPorIdUseCase
    {
        private readonly IPatoRepository _patoRepository;
        private readonly CaptureAssessmentService _assessmentService;
        public ObterPatoPorIdUseCase(IPatoRepository patoRepository, CaptureAssessmentService assessmentService)
        {
            // Atribui o repositório (com verificação de nulo)
            _patoRepository = patoRepository ?? throw new ArgumentNullException(nameof(patoRepository));
            // Atribui o serviço de avaliação (com verificação de nulo)
            _assessmentService = assessmentService ?? throw new ArgumentNullException(nameof(assessmentService)); 
        }

        public async Task<PatoDto?> HandleAsync(Guid id)
        {
            var pato = await _patoRepository.ObterPorIdAsync(id);

            if (pato == null)
            {
                return null; // Retorna null se não encontrar
            }
            var assessmentResult = _assessmentService.Assess(pato);

            // Mapeia a entidade para o DTO usando a função auxiliar
            var patoDto = MapToDto(pato); 

            // --- ADICIONE O RESULTADO DA AVALIAÇÃO AO DTO ---
            patoDto.RelatorioRisco = assessmentResult;

           
            // --- REUTILIZE A MESMA LÓGICA DE MAPEAMENTO ---
            // Se MapToDto fosse público, poderíamos chamá-lo diretamente.
            // Como é private, copiamos a lógica (ou refatoramos para um local comum).
            return patoDto; 
        }

        // --- COPIE EXATAMENTE A FUNÇÃO MapToDto DO ObterTodosPatosUseCase ---
        private static PatoDto MapToDto(PatoPrimordial created)
        {
            // Verifique se Poder pode ser null e adicione ?. se necessário
            return new PatoDto
            {
                Id = created.Id,
                DroneNumeroSerie = created.DroneNumeroSerie,
                AlturaValor = created.Altura.Valor,
                AlturaUnidade = created.Altura.UnidadeComprimento.ToString(),
                // Ajuste a linha do Peso se 'Em' for um método específico
                PesoValor = created.Peso.Em(created.Peso.UnidadeMassa),
                PesoUnidade = created.Peso.UnidadeMassa.ToString(),
                Pais = created.Localizacao.Pais,
                Cidade = created.Localizacao.Cidade,
                Latitude = created.Localizacao.Coordenada.Latitude,
                Longitude = created.Localizacao.Coordenada.Longitude,
                PontoReferencia = created.Localizacao.PontoReferencia, // Adicionei PontoReferencia
                Status = created.Status.ToString(),
                BatimentosPorMinuto = created.BatimentosPorMinuto,
                QuantidadeMutacoes = created.QuantidadeMutacoes,
                PoderNome = created.Poder.Nome,
                PoderDescricao = created.Poder.Descricao,
                PoderClassificacao = created.Poder.Classificacao, // Adicionado ?.
                DataColetaUtc = created.DataColetaUtc,
                Precisao = created.Localizacao.Precisao.Valor,
                PrecisaoUnidade = created.Localizacao.Precisao.UnidadeComprimento.ToString(),
                SequenciaDna = GeradorDna.GerarSequencia(created.QuantidadeMutacoes)
            };
        }
    }
}