using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Infrastructure.Data.Configurations
{
    public class DroneConfiguration : IEntityTypeConfiguration<DroneOperacional>
    {
        public void Configure(EntityTypeBuilder<DroneOperacional> builder)
        {
            builder.HasKey(d => d.NumeroSerie);
            
            builder.Property(d => d.NumeroSerie)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.Marca)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.Fabricante)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.PaisOrigem)
                .HasMaxLength(100)
                .IsRequired();

            // Configuração de propriedades complexas
            builder.OwnsOne(d => d.Bateria, bateria =>
            {
                bateria.Property(b => b.Valor).IsRequired();
                bateria.Property(b => b.ValorMaximo).IsRequired();
                bateria.Property(b => b.Unidade).IsRequired().HasMaxLength(10);
            });

            builder.OwnsOne(d => d.Combustivel, combustivel =>
            {
                combustivel.Property(c => c.Valor).IsRequired();
                combustivel.Property(c => c.ValorMaximo).IsRequired();
                combustivel.Property(c => c.Unidade).IsRequired().HasMaxLength(10);
            });

            builder.OwnsOne(d => d.IntegridadeFisica, integridade =>
            {
                integridade.Property(i => i.Valor).IsRequired();
                integridade.Property(i => i.ValorMaximo).IsRequired();
                integridade.Property(i => i.Unidade).IsRequired().HasMaxLength(10);
            });

            builder.OwnsOne(d => d.Posicao, posicao =>
            {
                posicao.Property(p => p.Latitude).IsRequired();
                posicao.Property(p => p.Longitude).IsRequired();
            });

            // Dados iniciais
            var bateria = new NivelRecurso(100, 100, "%");
            var combustivel = new NivelRecurso(100, 100, "%");
            var integridade = new NivelRecurso(100, 100, "%");
            var posicaoInicial = new Coordenada(0, 0);

            builder.HasData(
                new DroneOperacional("DRONE-001", "DJI", "DJI Technology", "China", bateria, combustivel, integridade, posicaoInicial),
                new DroneOperacional("DRONE-002", "Parrot", "Parrot SA", "França", bateria, combustivel, integridade, posicaoInicial),
                new DroneOperacional("DRONE-003", "Skydio", "Skydio Inc", "Estados Unidos", bateria, combustivel, integridade, posicaoInicial),
                new DroneOperacional("DRONE-004", "Autel", "Autel Robotics", "China", bateria, combustivel, integridade, posicaoInicial),
                new DroneOperacional("DRONE-005", "PowerVision", "PowerVision Tech", "China", bateria, combustivel, integridade, posicaoInicial)
            );
        }
    }
}