using Microsoft.EntityFrameworkCore;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;
using OperacaoPato.Backend.Infrastructure.Data.Configurations;

namespace OperacaoPato.Backend.Infrastructure.Data
{
    public class OperacaoPatoContext : DbContext
    {
        public OperacaoPatoContext(DbContextOptions<OperacaoPatoContext> options)
            : base(options)
        {
        }

        public DbSet<DroneOperacional> Drones { get; set; }
        public DbSet<PatoPrimordial> Patos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DroneConfiguration());
            modelBuilder.ApplyConfiguration(new PatoPrimordialConfiguration());
        }
    }
}