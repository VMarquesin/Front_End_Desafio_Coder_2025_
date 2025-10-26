// (usings no topo do arquivo)
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
            // --- PARTE 1: Configurações (Como estavam) ---
            builder.HasKey(d => d.NumeroSerie);
            builder.Property(d => d.NumeroSerie).HasMaxLength(50).IsRequired();
            builder.Property(d => d.Marca).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Fabricante).HasMaxLength(100).IsRequired();
            builder.Property(d => d.PaisOrigem).HasMaxLength(100).IsRequired();

            // --- PARTE 2: Definição dos Owned Types (Como estavam) ---
            // (Estou colapsando para economizar espaço, mantenha o seu código original aqui)
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

            // --- PARTE 3: Seeding (A CORREÇÃO) ---
            // 3a. Popula os Drones usando o inicializador de objeto {}
            // O EF Core usará o construtor 'private' que você me mostrou!
            var seedTime = new DateTime(2025, 10, 26, 0, 0, 0, DateTimeKind.Utc); // Isso é estático, está OK

            builder.HasData(
                new { 
                    Id = new Guid("11111111-1111-1111-1111-111111111111"), // <-- VALOR ESTÁTICO
                    NumeroSerie = "DRONE-001", Marca = "DJI", Fabricante = "DJI Technology", PaisOrigem = "China", 
                    AltitudeAtual = 0.0, VelocidadeAtual = 0.0, UltimaAtualizacao = seedTime 
                },
                new { 
                    Id = new Guid("22222222-2222-2222-2222-222222222222"), // <-- VALOR ESTÁTICO
                    NumeroSerie = "DRONE-002", Marca = "Parrot", Fabricante = "Parrot SA", PaisOrigem = "França", 
                    AltitudeAtual = 0.0, VelocidadeAtual = 0.0, UltimaAtualizacao = seedTime 
                },
                new { 
                    Id = new Guid("33333333-3333-3333-3333-333333333333"), // <-- VALOR ESTÁTICO
                    NumeroSerie = "DRONE-003", Marca = "Skydio", Fabricante = "Skydio Inc", PaisOrigem = "Estados Unidos", 
                    AltitudeAtual = 0.0, VelocidadeAtual = 0.0, UltimaAtualizacao = seedTime 
                },
                new { 
                    Id = new Guid("44444444-4444-4444-4444-444444444444"), // <-- VALOR ESTÁTICO
                    NumeroSerie = "DRONE-004", Marca = "Autel", Fabricante = "Autel Robotics", PaisOrigem = "China", 
                    AltitudeAtual = 0.0, VelocidadeAtual = 0.0, UltimaAtualizacao = seedTime 
                },
                new { 
                    Id = new Guid("55555555-5555-5555-5555-555555555555"), // <-- VALOR ESTÁTICO
                    NumeroSerie = "DRONE-005", Marca = "PowerVision", Fabricante = "PowerVision Tech", PaisOrigem = "China", 
                    AltitudeAtual = 0.0, VelocidadeAtual = 0.0, UltimaAtualizacao = seedTime 
                }
            );
            // 3b. Popula a BATERIA (Este código já estava correto)
            builder.OwnsOne(d => d.Bateria).HasData(
                new { DroneOperacionalNumeroSerie = "DRONE-001", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-002", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-003", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-004", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-005", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" }
            );
            
            // 3c. Popula o COMBUSTÍVEL (Este código já estava correto)
            builder.OwnsOne(d => d.Combustivel).HasData(
                new { DroneOperacionalNumeroSerie = "DRONE-001", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-002", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-003", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-004", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-005", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" }
            );

            // 3d. Popula a INTEGRIDADE FÍSICA (Este código já estava correto)
            builder.OwnsOne(d => d.IntegridadeFisica).HasData(
                new { DroneOperacionalNumeroSerie = "DRONE-001", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-002", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-003", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-004", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" },
                new { DroneOperacionalNumeroSerie = "DRONE-005", Valor = 100.0, ValorMaximo = 100.0, Unidade = "%" }
            );
            
            // 3e. Popula a POSIÇÃO (Este código já estava correto)
            builder.OwnsOne(d => d.Posicao).HasData(
                new { DroneOperacionalNumeroSerie = "DRONE-001", Latitude = 0.0, Longitude = 0.0 },
                new { DroneOperacionalNumeroSerie = "DRONE-002", Latitude = 0.0, Longitude = 0.0 },
                new { DroneOperacionalNumeroSerie = "DRONE-003", Latitude = 0.0, Longitude = 0.0 },
                new { DroneOperacionalNumeroSerie = "DRONE-004", Latitude = 0.0, Longitude = 0.0 },
                new { DroneOperacionalNumeroSerie = "DRONE-005", Latitude = 0.0, Longitude = 0.0 }
            );
        }
    }
}