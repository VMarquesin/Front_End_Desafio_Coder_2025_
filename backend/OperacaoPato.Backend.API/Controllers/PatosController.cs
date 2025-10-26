using Microsoft.AspNetCore.Mvc;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Services; // Mantido se Assess usa
using OperacaoPato.Backend.Application.UseCases.CadastrarPato; // UseCase existente
using OperacaoPato.Backend.Application.UseCases.ObterTodosPatos; // UseCase existente
using OperacaoPato.Backend.Application.Interfaces; // Para IPatoRepository e a nova interface
// --- ADICIONE O using PARA A NOVA INTERFACE ---
using OperacaoPato.Backend.Application.UseCases.ObterPatoPorId; 

namespace OperacaoPato.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Rota base é /api/Patos
    public class PatosController : ControllerBase
    {
        private readonly ICadastrarPatoUseCase _cadastrarPato;
        private readonly IObterTodosPatosUseCase _obterTodosPatos;
        private readonly IPatoRepository _patoRepository; // Mantido para o Assess
        private readonly CaptureAssessmentService _assessmentService; // Mantido para o Assess
        
        // --- ADICIONE A NOVA INTERFACE ---
        private readonly IObterPatoPorIdUseCase _obterPatoPorId; 

        // Construtor atualizado para receber a nova interface
        public PatosController(
            ICadastrarPatoUseCase cadastrarPato,
            IObterTodosPatosUseCase obterTodosPatos,
            IPatoRepository patoRepository,
            CaptureAssessmentService assessmentService,
            // --- INJETE A NOVA INTERFACE ---
            IObterPatoPorIdUseCase obterPatoPorId) 
        {
            _cadastrarPato = cadastrarPato;
            _obterTodosPatos = obterTodosPatos;
            _patoRepository = patoRepository;
            _assessmentService = assessmentService;
            // --- ATRIBUA A NOVA INTERFACE ---
            _obterPatoPorId = obterPatoPorId; 
        }

        // GET: /api/Patos (Sem alteração)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatoDto>>> ObterTodos()
        {
            var patos = await _obterTodosPatos.HandleAsync();
            return Ok(patos);
        }

        // --- ADICIONE O NOVO MÉTODO GET POR ID ---
        // GET: /api/Patos/{id}
        [HttpGet("{id:guid}")] // Define a rota como /api/Patos/GUID
        public async Task<ActionResult<PatoDto>> ObterPorId(Guid id)
        {
            var patoDto = await _obterPatoPorId.HandleAsync(id); // Chama o Use Case

            if (patoDto == null)
            {
                // Retorna 404 se o Use Case não encontrou o pato
                return NotFound($"Pato com ID {id} não encontrado."); 
            }

            // Retorna o DTO com status 200 OK
            return Ok(patoDto); 
        }

        // POST: /api/Patos (Sem alteração - mas com sugestão de melhoria no CreatedAtAction)
        [HttpPost]
        public async Task<ActionResult<PatoDto>> Cadastrar([FromBody] PatoDto dto)
        {
            // Atenção: Receber o PatoDto completo aqui pode ser um problema se ele tiver campos
            // que o usuário não deveria definir na criação (como o Id).
            // Idealmente, você teria um CreatePatoDto.
            var criado = await _cadastrarPato.HandleAsync(dto); 
            
            // Verifique se 'criado' tem a propriedade Id para usar no CreatedAtAction
            // Se 'criado' for do tipo PatoDto (como parece ser), isso deve funcionar.
            var idDoCriado = (criado as PatoDto)?.Id; 
            
            // Retorna 201 Created com a URL para o novo recurso (usando ObterPorId)
            return CreatedAtAction(nameof(ObterPorId), new { id = idDoCriado }, criado); 
        }

        // POST: /api/Patos/{id}/assess (Sem alteração)
        [HttpPost("{id:guid}/assess")]
        public async Task<ActionResult<CaptureAssessmentResult>> Assess(Guid id)
        {
            // Este método ainda usa o repositório diretamente. 
            // Poderia ser refatorado para um Use Case também, se quisessem.
            var pato = await _patoRepository.ObterPorIdAsync(id); 
            if (pato == null) return NotFound($"Pato com ID {id} não encontrado para avaliação.");

            var result = _assessmentService.Assess(pato);
            return Ok(result);
        }
    }
}