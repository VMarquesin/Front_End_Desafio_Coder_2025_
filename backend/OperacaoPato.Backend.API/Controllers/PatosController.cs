using Microsoft.AspNetCore.Mvc;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Services;
using OperacaoPato.Backend.Application.UseCases.CadastrarPato;
using OperacaoPato.Backend.Application.UseCases.ObterTodosPatos;
using OperacaoPato.Backend.Application.Interfaces;

namespace OperacaoPato.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatosController : ControllerBase
    {
    private readonly ICadastrarPatoUseCase _cadastrarPato;
    private readonly IObterTodosPatosUseCase _obterTodosPatos;
    private readonly IPatoRepository _patoRepository;
    private readonly CaptureAssessmentService _assessmentService;

        public PatosController(
            ICadastrarPatoUseCase cadastrarPato,
            IObterTodosPatosUseCase obterTodosPatos,
            IPatoRepository patoRepository,
            CaptureAssessmentService assessmentService)
        {
            _cadastrarPato = cadastrarPato;
            _obterTodosPatos = obterTodosPatos;
            _patoRepository = patoRepository;
            _assessmentService = assessmentService;
        }

        // GET: /api/patos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatoDto>>> ObterTodos()
        {
            var patos = await _obterTodosPatos.HandleAsync();
            return Ok(patos);
        }

        // POST: /api/patos
        [HttpPost]
        public async Task<ActionResult<PatoDto>> Cadastrar([FromBody] PatoDto dto)
        {
            var criado = await _cadastrarPato.HandleAsync(dto);
            return CreatedAtAction(nameof(ObterTodos), null, criado);
        }

        // POST: /api/patos/{id}/assess
        [HttpPost("{id:guid}/assess")]
        public async Task<ActionResult<CaptureAssessmentResult>> Assess(Guid id)
        {
            var pato = await _patoRepository.ObterPorIdAsync(id);
            if (pato == null) return NotFound();

            var result = _assessmentService.Assess(pato);
            return Ok(result);
        }
    }
}