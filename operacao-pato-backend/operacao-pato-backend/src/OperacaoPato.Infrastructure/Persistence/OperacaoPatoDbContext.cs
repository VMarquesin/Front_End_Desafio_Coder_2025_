using Microsoft.EntityFrameworkCore;
using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Infrastructure.Persistence
{
    public class OperacaoPatoDbContext : DbContext
    {
        public OperacaoPatoDbContext(DbContextOptions<OperacaoPatoDbContext> options) : base(options)
        {
        }

        public DbSet<PatoPrimordial> PatosPrimordiais { get; set; }
        public DbSet<Drone> Drones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity properties and relationships here
        }
    }
}