using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OperacaoPato.Domain.Entities;
using OperacaoPato.Application.Interfaces;

namespace OperacaoPato.Infrastructure.Persistence
{
    public class PatoRepository : IPatoRepository
    {
        private readonly OperacaoPatoDbContext _context;

        public PatoRepository(OperacaoPatoDbContext context)
        {
            _context = context;
        }

        public async Task<PatoPrimordial> GetByIdAsync(int id)
        {
            return await _context.PatosPrimordiais.FindAsync(id);
        }

        public async Task<IEnumerable<PatoPrimordial>> GetAllAsync()
        {
            return await _context.PatosPrimordiais.ToListAsync();
        }

        public async Task AddAsync(PatoPrimordial pato)
        {
            await _context.PatosPrimordiais.AddAsync(pato);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PatoPrimordial pato)
        {
            _context.PatosPrimordiais.Update(pato);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pato = await GetByIdAsync(id);
            if (pato != null)
            {
                _context.PatosPrimordiais.Remove(pato);
                await _context.SaveChangesAsync();
            }
        }
    }
}