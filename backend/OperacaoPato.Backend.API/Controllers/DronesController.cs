using Microsoft.AspNetCore.Mvc;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Services;
using OperacaoPato.Backend.Application.UseCases.CadastrarDrone;

namespace OperacaoPato.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DronesController : ControllerBase
    {
        private readonly IDroneService _service;
        private readonly CadastrarDroneUseCase _cadastrarDrone;

        public DronesController(IDroneService service, CadastrarDroneUseCase cadastrarDrone)
        {
            _service = service;
            _cadastrarDrone = cadastrarDrone;
        }

        // GET: /api/drones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DroneDto>>> ObterTodos()
        {
            var drones = await _service.ObterTodosAsync();
            return Ok(drones);
        }

        // POST: /api/drones
        [HttpPost]
        public async Task<ActionResult<DroneDto>> Cadastrar([FromBody] DroneDto dto)
        {
            if (dto is null) return BadRequest("Payload inválido.");
            var criado = await _cadastrarDrone.HandleAsync(dto);
            return CreatedAtAction(nameof(ObterTodos), null, criado);
        }

        // POST: /api/drones/lote
        [HttpPost("lote")]
        public async Task<ActionResult<IEnumerable<DroneDto>>> CadastrarLote([FromBody] IEnumerable<DroneDto> dtos)
        {
            if (dtos is null) return BadRequest("Payload inválido.");
            var criados = new List<DroneDto>();
            foreach (var d in dtos)
            {
                criados.Add(await _cadastrarDrone.HandleAsync(d));
            }
            return CreatedAtAction(nameof(ObterTodos), null, criados);
        }
    }
}