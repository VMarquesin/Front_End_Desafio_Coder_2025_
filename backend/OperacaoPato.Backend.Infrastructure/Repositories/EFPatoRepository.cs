using Microsoft.EntityFrameworkCore;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Infrastructure.Data;

namespace OperacaoPato.Backend.Infrastructure.Repositories
{
    public class EFPatoRepository : IPatoRepository
    {
        private readonly OperacaoPatoContext _context;

        public EFPatoRepository(OperacaoPatoContext context)
        {
            _context = context;
        }

        public async Task<PatoPrimordial> AdicionarAsync(PatoPrimordial pato)
        {
            await _context.Patos.AddAsync(pato);
            await _context.SaveChangesAsync();
            return pato;
        }

        public async Task<bool> ExisteAsync(string droneNumeroSerie, DateTime dataColetaUtc)
        {
            return await _context.Patos
                .AnyAsync(p => p.DroneNumeroSerie == droneNumeroSerie && 
                              p.DataColetaUtc == dataColetaUtc);
        }

        public async Task<PatoPrimordial?> ObterPorIdAsync(Guid id)
        {
            return await _context.Patos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PatoPrimordial>> ObterTodosAsync()
        {
            return await _context.Patos.ToListAsync();
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var pato = await ObterPorIdAsync(id);
            if (pato == null) return false;

            _context.Patos.Remove(pato);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}