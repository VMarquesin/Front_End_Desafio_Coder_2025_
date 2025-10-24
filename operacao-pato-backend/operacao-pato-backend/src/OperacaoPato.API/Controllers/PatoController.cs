using Microsoft.AspNetCore.Mvc;
using OperacaoPato.API.Models;
using OperacaoPato.Application.Services;
using OperacaoPato.Application.DTOs;

namespace OperacaoPato.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatoController : ControllerBase
    {
        private readonly PatoCatalogService _service;

        public PatoController(PatoCatalogService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Catalogar([FromBody] PatoInputModel input)
        {
            var dto = input.ToDto();
            var criado = _service.CatalogarPato(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(Guid id)
        {
            var pato = _service.ObterPorId(id);
            if (pato == null) return NotFound();
            return Ok(pato);
        }
    }
}