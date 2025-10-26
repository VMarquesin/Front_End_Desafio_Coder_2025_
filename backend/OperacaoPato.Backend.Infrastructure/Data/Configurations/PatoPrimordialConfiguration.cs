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
            // --- PARTE 3: Seeding (A CORREÇÃO - Separando HasData) ---
            
            // IDs estáticos
            var pato1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var pato2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var pato3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Data de coleta fixa
            var dataColeta1 = DateTime.Parse("2025-10-23T12:00:00Z").ToUniversalTime();
            var dataColeta2 = DateTime.Parse("2025-10-24T12:00:00Z").ToUniversalTime();
            var dataColeta3 = DateTime.Parse("2025-10-25T12:00:00Z").ToUniversalTime();

            // 3a. Popula os Patos (APENAS propriedades diretas)
            builder.HasData(
                new { 
                    Id = pato1Id, 
                    DroneNumeroSerie = "DRONE-001",
                    Status = StatusHibernacao.Desperto,
                    BatimentosPorMinuto = (int?)null,
                    QuantidadeMutacoes = 2,
                    DataColetaUtc = dataColeta1
                },
                new { 
                    Id = pato2Id, 
                    DroneNumeroSerie = "DRONE-002",
                    Status = StatusHibernacao.Transe,
                    BatimentosPorMinuto = 30,
                    QuantidadeMutacoes = 3,
                    DataColetaUtc = dataColeta2
                },
                new { 
                    Id = pato3Id, 
                    DroneNumeroSerie = "DRONE-003",
                    Status = StatusHibernacao.HibernacaoProfunda,
                    BatimentosPorMinuto = 15,
                    QuantidadeMutacoes = 5,
                    DataColetaUtc = dataColeta3
                }
            );

            // 3b. Popula ALTURA (Owned Type) separadamente
            // Note o nome da chave estrangeira gerado pelo EF: PatoPrimordialId
            builder.OwnsOne(p => p.Altura).HasData(
                new { PatoPrimordialId = pato1Id, Valor = 50.0, UnidadeComprimento = UnidadeComprimento.Centimetro }, // Valor = Altura_Valor, UnidadeComprimento = Altura_Unidade
                new { PatoPrimordialId = pato2Id, Valor = 45.0, UnidadeComprimento = UnidadeComprimento.Centimetro },
                new { PatoPrimordialId = pato3Id, Valor = 55.0, UnidadeComprimento = UnidadeComprimento.Centimetro }
            );

            // 3c. Popula PESO (Owned Type) separadamente
            builder.OwnsOne(p => p.Peso).HasData(
                new { PatoPrimordialId = pato1Id, Valor = 2500.0, UnidadeMassa = UnidadeMassa.Grama }, // Valor = Peso_Valor, UnidadeMassa = Peso_Unidade
                new { PatoPrimordialId = pato2Id, Valor = 2200.0, UnidadeMassa = UnidadeMassa.Grama },
                new { PatoPrimordialId = pato3Id, Valor = 3000.0, UnidadeMassa = UnidadeMassa.Grama }
            );

            // 3d. Popula LOCALIZACAO (Owned Type) separadamente
            // Note a chave estrangeira aqui também
            builder.OwnsOne(p => p.Localizacao).HasData(
                new { PatoPrimordialId = pato1Id, Cidade = "Manaus", Pais = "Brasil", PontoReferencia = "Encontro das Águas" },
                new { PatoPrimordialId = pato2Id, Cidade = "Rio de Janeiro", Pais = "Brasil", PontoReferencia = "Pão de Açúcar" },
                new { PatoPrimordialId = pato3Id, Cidade = "São Paulo", Pais = "Brasil", PontoReferencia = "Parque Ibirapuera" }
            );

            // 3e. Popula COORDENADA (Owned dentro de Localizacao) separadamente
            // Note a chave estrangeira aninhada: LocalizacaoPatoPrimordialId
            builder.OwnsOne(p => p.Localizacao).OwnsOne(l => l.Coordenada).HasData(
                 new { LocalizacaoPatoPrimordialId = pato1Id, Latitude = -3.1190, Longitude = -60.0217 }, // Latitude = Localizacao_Latitude, etc.
                 new { LocalizacaoPatoPrimordialId = pato2Id, Latitude = -22.9068, Longitude = -43.1729 },
                 new { LocalizacaoPatoPrimordialId = pato3Id, Latitude = -23.5505, Longitude = -46.6333 }
            );

            // 3f. Popula PRECISÃO (Owned dentro de Localizacao) separadamente
            builder.OwnsOne(p => p.Localizacao).OwnsOne(l => l.Precisao).HasData(
                 new { LocalizacaoPatoPrimordialId = pato1Id, Valor = 5.0, UnidadeComprimento = UnidadeComprimento.Metro }, // Valor = Localizacao_Precisao_Valor, etc.
                 new { LocalizacaoPatoPrimordialId = pato2Id, Valor = 10.0, UnidadeComprimento = UnidadeComprimento.Metro },
                 new { LocalizacaoPatoPrimordialId = pato3Id, Valor = 8.0, UnidadeComprimento = UnidadeComprimento.Metro }
            );

            // 3g. Popula PODER (Owned Type) separadamente - Note que só precisamos dos patos que TÊM poder
            builder.OwnsOne(p => p.Poder).HasData(
                new { PatoPrimordialId = pato1Id, Nome = "Rajada de Água", Descricao = "Dispara jatos de água pressurizada", Classificacao = "ofensivo" }, // Nome = Poder_Nome, etc.
                new { PatoPrimordialId = pato2Id, Nome = "Camuflagem Aquática", Descricao = "Torna-se invisível na água", Classificacao = "defensivo" },
                new { PatoPrimordialId = pato3Id, Nome = "Voo Supersônico", Descricao = "Voa em velocidades extremas", Classificacao = "mobilidade" }
            );
        }
    }
}