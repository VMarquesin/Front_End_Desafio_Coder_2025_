using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Application.Services
{
    public interface IDroneService
    {
        // Consulta
        Task<IEnumerable<DroneDto>> ObterTodosAsync();

        // Cadastro unitário
        Task<DroneDto> CadastrarAsync(DroneDto droneDto);

        // Cadastro em lote
        Task<IEnumerable<DroneDto>> CadastrarLoteAsync(IEnumerable<DroneDto> dtos);

        // Remoção
        Task<bool> ApagarDroneAsync(string numeroSerie);
    }
}