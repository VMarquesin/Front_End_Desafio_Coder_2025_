using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Backend.Application.Interfaces
{
    public interface IDroneRepository
    {
        Task<Drone?> ObterPorNumeroSerieAsync(string numeroSerie);
        Task<IEnumerable<Drone>> ObterTodosAsync();
        Task<bool> ExisteAsync(string numeroSerie);
        Task<Drone> AdicionarAsync(Drone drone);
        Task<bool> RemoverAsync(string numeroSerie);
    }
}