using System.Collections.Concurrent;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Infrastructure.Repositories
{
    public class InMemoryDroneRepository : IDroneRepository
    {
        private readonly ConcurrentDictionary<string, DroneOperacional> _drones = new();

        public async Task<DroneOperacional> AdicionarAsync(DroneOperacional drone)
        {
            if (!_drones.TryAdd(drone.NumeroSerie, drone))
            {
                throw new InvalidOperationException($"Drone com número de série {drone.NumeroSerie} já existe.");
            }

            return await Task.FromResult(drone);
        }

        public async Task<bool> ApagarAsync(string numeroSerie)
        {
            return await Task.FromResult(_drones.TryRemove(numeroSerie, out _));
        }

        public async Task<bool> ExisteAsync(string numeroSerie)
        {
            return await Task.FromResult(_drones.ContainsKey(numeroSerie));
        }

        public async Task<DroneOperacional?> ObterPorNumeroSerieAsync(string numeroSerie)
        {
            _drones.TryGetValue(numeroSerie, out var drone);
            return await Task.FromResult(drone);
        }

        public async Task<IEnumerable<DroneOperacional>> ObterTodosAsync()
        {
            return await Task.FromResult(_drones.Values.ToList());
        }

        public async Task<bool> RemoverAsync(string numeroSerie)
        {
            return await Task.FromResult(_drones.TryRemove(numeroSerie, out _));
        }
    }
}