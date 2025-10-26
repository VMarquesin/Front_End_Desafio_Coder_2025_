using Microsoft.EntityFrameworkCore;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Infrastructure.Data;

namespace OperacaoPato.Backend.Infrastructure.Repositories
{
    public class EFDroneRepository : IDroneRepository
    {
        private readonly OperacaoPatoContext _context;

        public EFDroneRepository(OperacaoPatoContext context)
        {
            _context = context;
        }

        public async Task<DroneOperacional> AdicionarAsync(DroneOperacional drone)
        {
            await _context.Drones.AddAsync(drone);
            await _context.SaveChangesAsync();
            return drone;
        }

        public async Task<bool> ExisteAsync(string numeroSerie)
        {
            return await _context.Drones
                .AnyAsync(d => d.NumeroSerie == numeroSerie);
        }

        public async Task<DroneOperacional?> ObterPorNumeroSerieAsync(string numeroSerie)
        {
            return await _context.Drones
                .FirstOrDefaultAsync(d => d.NumeroSerie == numeroSerie);
        }

        public async Task<IEnumerable<DroneOperacional>> ObterTodosAsync()
        {
            return await _context.Drones.ToListAsync();
        }

        public async Task<bool> RemoverAsync(string numeroSerie)
        {
            var drone = await ObterPorNumeroSerieAsync(numeroSerie);
            if (drone == null) return false;

            _context.Drones.Remove(drone);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}