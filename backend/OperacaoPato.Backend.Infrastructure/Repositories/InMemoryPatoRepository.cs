using System.Collections.Concurrent;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Infrastructure.Repositories
{
    public class InMemoryPatoRepository : IPatoRepository
    {
        private readonly ConcurrentDictionary<Guid, PatoPrimordial> _store = new();

        public Task<PatoPrimordial> AdicionarAsync(PatoPrimordial pato)
        {
            _store[pato.Id] = pato;
            return Task.FromResult(pato);
        }

        public Task<IEnumerable<PatoPrimordial>> ObterTodosAsync()
        {
            return Task.FromResult(_store.Values.AsEnumerable());
        }

        public Task<PatoPrimordial?> ObterPorIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var pato);
            return Task.FromResult(pato);
        }

        public Task<bool> ExisteAsync(string droneNumeroSerie, DateTime dataColetaUtc)
        {
            var exists = _store.Values.Any(p =>
                p.DroneNumeroSerie.Equals(droneNumeroSerie, StringComparison.OrdinalIgnoreCase) &&
                p.DataColetaUtc == dataColetaUtc.ToUniversalTime());
            return Task.FromResult(exists);
        }

        public Task<bool> RemoverAsync(Guid id)
        {
            return Task.FromResult(_store.TryRemove(id, out _));
        }
    }
}