using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Backend.Application.Services
{
    public interface IDroneService
    {
        // Consulta
        Task<IEnumerable<DroneDto>> ObterTodosAsync();

        // Cadastro unitário (retorna DTO para a API)
        Task<DroneDto> CadastrarAsync(DroneDto droneDto);

        // Cadastro em lote (retorna DTOs para a API)
        Task<IEnumerable<DroneDto>> CadastrarLoteAsync(IEnumerable<DroneDto> dtos);

        // Métodos existentes (mantidos para compatibilidade, se usados em outros lugares)
        Task<Drone> CadastrarDroneAsync(DroneDto droneDto);
        Task<bool> ApagarDroneAsync(string numeroSerie);
    }
}