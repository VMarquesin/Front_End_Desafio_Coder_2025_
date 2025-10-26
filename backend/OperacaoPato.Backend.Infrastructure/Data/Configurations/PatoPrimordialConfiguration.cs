using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Enums;

using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Infrastructure.Data.Configurations
{
    public class PatoPrimordialConfiguration : IEntityTypeConfiguration<PatoPrimordial>
    {
        public void Configure(EntityTypeBuilder<PatoPrimordial> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DroneNumeroSerie)
                .HasMaxLength(50)
                .IsRequired();

            // Configuração de Value Objects
            builder.OwnsOne(p => p.Altura, altura =>
            {
                altura.Property(a => a.Valor).HasColumnName("Altura_Valor");
                altura.Property(a => a.UnidadeComprimento).HasColumnName("Altura_Unidade");
            });

            builder.OwnsOne(p => p.Peso, peso =>
            {
                peso.Property(p => p.Valor).HasColumnName("Peso_Valor");
                peso.Property(p => p.UnidadeMassa).HasColumnName("Peso_Unidade");
            });

            builder.OwnsOne(p => p.Localizacao, loc =>
            {
                loc.Property(l => l.Cidade).HasMaxLength(100).IsRequired().HasColumnName("Localizacao_Cidade");
                loc.Property(l => l.Pais).HasMaxLength(100).IsRequired().HasColumnName("Localizacao_Pais");
                loc.Property(l => l.PontoReferencia).HasMaxLength(200).HasColumnName("Localizacao_PontoReferencia");

                // Precisao (Comprimento) mapeada como colunas internas
                loc.OwnsOne(l => l.Precisao, precisao =>
                {
                    precisao.Property(p => p.Valor).HasColumnName("Localizacao_Precisao_Valor");
                    precisao.Property(p => p.UnidadeComprimento).HasColumnName("Localizacao_Precisao_Unidade");
                });

                loc.OwnsOne(l => l.Coordenada, coord =>
                {
                    coord.Property(c => c.Latitude).HasColumnName("Localizacao_Latitude");
                    coord.Property(c => c.Longitude).HasColumnName("Localizacao_Longitude");
                });
            });

            builder.OwnsOne(p => p.Poder, poder =>
            {
                poder.Property(p => p.Nome).HasMaxLength(100).IsRequired().HasColumnName("Poder_Nome");
                poder.Property(p => p.Descricao).HasMaxLength(500).HasColumnName("Poder_Descricao");
                poder.Property(p => p.Classificacao).HasMaxLength(50).HasColumnName("Poder_Classificacao");
            });

            // Dados iniciais (usando objetos anônimos para HasData — necessário para tipos owned / construtores complexos)
            builder.HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DroneNumeroSerie = "DRONE-001",
                    Altura_Valor = 50.0,
                    Altura_Unidade = 1, // Centimetro
                    Peso_Valor = 2500.0,
                    Peso_Unidade = 0, // Grama
                    Localizacao_Cidade = "Manaus",
                    Localizacao_Pais = "Brasil",
                    Localizacao_PontoReferencia = "Encontro das Águas",
                    Localizacao_Latitude = -3.1190,
                    Localizacao_Longitude = -60.0217,
                    Localizacao_Precisao_Valor = 5.0,
                    Localizacao_Precisao_Unidade = 0, // Metro
                    Poder_Nome = "Rajada de Água",
                    Poder_Descricao = "Dispara jatos de água pressurizada",
                    Poder_Classificacao = "ofensivo",
                    Status = StatusHibernacao.Desperto,
                    BatimentosPorMinuto = (int?)null,
                    QuantidadeMutacoes = 2,
                    DataColetaUtc = DateTime.Parse("2025-10-23T12:00:00Z")
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DroneNumeroSerie = "DRONE-002",
                    Altura_Valor = 45.0,
                    Altura_Unidade = 1,
                    Peso_Valor = 2200.0,
                    Peso_Unidade = 0,
                    Localizacao_Cidade = "Rio de Janeiro",
                    Localizacao_Pais = "Brasil",
                    Localizacao_PontoReferencia = "Pão de Açúcar",
                    Localizacao_Latitude = -22.9068,
                    Localizacao_Longitude = -43.1729,
                    Localizacao_Precisao_Valor = 10.0,
                    Localizacao_Precisao_Unidade = 0,
                    Poder_Nome = "Camuflagem Aquática",
                    Poder_Descricao = "Torna-se invisível na água",
                    Poder_Classificacao = "defensivo",
                    Status = StatusHibernacao.Transe,
                    BatimentosPorMinuto = 30,
                    QuantidadeMutacoes = 3,
                    DataColetaUtc = DateTime.Parse("2025-10-24T12:00:00Z")
                },
                new
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    DroneNumeroSerie = "DRONE-003",
                    Altura_Valor = 55.0,
                    Altura_Unidade = 1,
                    Peso_Valor = 3000.0,
                    Peso_Unidade = 0,
                    Localizacao_Cidade = "São Paulo",
                    Localizacao_Pais = "Brasil",
                    Localizacao_PontoReferencia = "Parque Ibirapuera",
                    Localizacao_Latitude = -23.5505,
                    Localizacao_Longitude = -46.6333,
                    Localizacao_Precisao_Valor = 8.0,
                    Localizacao_Precisao_Unidade = 0,
                    Poder_Nome = "Voo Supersônico",
                    Poder_Descricao = "Voa em velocidades extremas",
                    Poder_Classificacao = "mobilidade",
                    Status = StatusHibernacao.HibernacaoProfunda,
                    BatimentosPorMinuto = 15,
                    QuantidadeMutacoes = 5,
                    DataColetaUtc = DateTime.Parse("2025-10-25T12:00:00Z")
                }
            );
        }
    }
}