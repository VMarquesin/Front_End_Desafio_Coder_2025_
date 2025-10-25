using Microsoft.AspNetCore.Mvc;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.UseCases.CadastrarPato;
using OperacaoPato.Backend.Application.UseCases.ObterTodosPatos;

namespace OperacaoPato.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatosController : ControllerBase
    {
        private readonly ICadastrarPatoUseCase _cadastrarPato;
        private readonly IObterTodosPatosUseCase _obterTodosPatos;

        public PatosController(
            ICadastrarPatoUseCase cadastrarPato,
            IObterTodosPatosUseCase obterTodosPatos)
        {
            _cadastrarPato = cadastrarPato;
            _obterTodosPatos = obterTodosPatos;
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
    }
}