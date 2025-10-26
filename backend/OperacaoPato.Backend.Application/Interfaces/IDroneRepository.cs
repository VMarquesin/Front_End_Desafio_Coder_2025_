using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Application.Interfaces
{
    public interface IDroneRepository
    {
        Task<DroneOperacional?> ObterPorNumeroSerieAsync(string numeroSerie);
        Task<IEnumerable<DroneOperacional>> ObterTodosAsync();
        Task<bool> ExisteAsync(string numeroSerie);
        Task<DroneOperacional> AdicionarAsync(DroneOperacional drone);
        Task<bool> RemoverAsync(string numeroSerie);
    }
}