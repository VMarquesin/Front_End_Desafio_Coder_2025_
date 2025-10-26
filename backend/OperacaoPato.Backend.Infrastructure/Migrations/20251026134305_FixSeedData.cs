using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OperacaoPato.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    NumeroSerie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bateria_Valor = table.Column<double>(type: "float", nullable: false),
                    Bateria_ValorMaximo = table.Column<double>(type: "float", nullable: false),
                    Bateria_Unidade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Combustivel_Valor = table.Column<double>(type: "float", nullable: false),
                    Combustivel_ValorMaximo = table.Column<double>(type: "float", nullable: false),
                    Combustivel_Unidade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IntegridadeFisica_Valor = table.Column<double>(type: "float", nullable: false),
                    IntegridadeFisica_ValorMaximo = table.Column<double>(type: "float", nullable: false),
                    IntegridadeFisica_Unidade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Posicao_Latitude = table.Column<double>(type: "float", nullable: false),
                    Posicao_Longitude = table.Column<double>(type: "float", nullable: false),
                    VelocidadeAtual = table.Column<double>(type: "float", nullable: false),
                    AltitudeAtual = table.Column<double>(type: "float", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fabricante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaisOrigem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drones", x => x.NumeroSerie);
                });

            migrationBuilder.CreateTable(
                name: "Patos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DroneNumeroSerie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Altura_Valor = table.Column<double>(type: "float", nullable: false),
                    Altura_Unidade = table.Column<int>(type: "int", nullable: false),
                    Peso_Valor = table.Column<double>(type: "float", nullable: false),
                    Peso_Unidade = table.Column<int>(type: "int", nullable: false),
                    Localizacao_Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Localizacao_Pais = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Localizacao_Latitude = table.Column<double>(type: "float", nullable: false),
                    Localizacao_Longitude = table.Column<double>(type: "float", nullable: false),
                    Localizacao_Precisao_Valor = table.Column<double>(type: "float", nullable: false),
                    Localizacao_Precisao_Unidade = table.Column<int>(type: "int", nullable: false),
                    Localizacao_PontoReferencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BatimentosPorMinuto = table.Column<int>(type: "int", nullable: true),
                    QuantidadeMutacoes = table.Column<int>(type: "int", nullable: false),
                    Poder_Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Poder_Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Poder_Classificacao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataColetaUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Drones",
                columns: new[] { "NumeroSerie", "AltitudeAtual", "Fabricante", "Id", "Marca", "PaisOrigem", "UltimaAtualizacao", "VelocidadeAtual", "Bateria_Unidade", "Bateria_Valor", "Bateria_ValorMaximo", "Combustivel_Unidade", "Combustivel_Valor", "Combustivel_ValorMaximo", "IntegridadeFisica_Unidade", "IntegridadeFisica_Valor", "IntegridadeFisica_ValorMaximo", "Posicao_Latitude", "Posicao_Longitude" },
                values: new object[,]
                {
                    { "DRONE-001", 0.0, "DJI Technology", new Guid("11111111-1111-1111-1111-111111111111"), "DJI", "China", new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "%", 100.0, 100.0, "%", 100.0, 100.0, "%", 100.0, 100.0, 0.0, 0.0 },
                    { "DRONE-002", 0.0, "Parrot SA", new Guid("22222222-2222-2222-2222-222222222222"), "Parrot", "França", new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "%", 100.0, 100.0, "%", 100.0, 100.0, "%", 100.0, 100.0, 0.0, 0.0 },
                    { "DRONE-003", 0.0, "Skydio Inc", new Guid("33333333-3333-3333-3333-333333333333"), "Skydio", "Estados Unidos", new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "%", 100.0, 100.0, "%", 100.0, 100.0, "%", 100.0, 100.0, 0.0, 0.0 },
                    { "DRONE-004", 0.0, "Autel Robotics", new Guid("44444444-4444-4444-4444-444444444444"), "Autel", "China", new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "%", 100.0, 100.0, "%", 100.0, 100.0, "%", 100.0, 100.0, 0.0, 0.0 },
                    { "DRONE-005", 0.0, "PowerVision Tech", new Guid("55555555-5555-5555-5555-555555555555"), "PowerVision", "China", new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "%", 100.0, 100.0, "%", 100.0, 100.0, "%", 100.0, 100.0, 0.0, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "Patos",
                columns: new[] { "Id", "BatimentosPorMinuto", "DataColetaUtc", "DroneNumeroSerie", "QuantidadeMutacoes", "Status", "Altura_Unidade", "Altura_Valor", "Poder_Classificacao", "Poder_Descricao", "Poder_Nome", "Localizacao_Cidade", "Localizacao_Pais", "Localizacao_PontoReferencia", "Localizacao_Latitude", "Localizacao_Longitude", "Localizacao_Precisao_Unidade", "Localizacao_Precisao_Valor", "Peso_Unidade", "Peso_Valor" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2025, 10, 23, 12, 0, 0, 0, DateTimeKind.Utc), "DRONE-001", 2, 0, 1, 50.0, "ofensivo", "Dispara jatos de água pressurizada", "Rajada de Água", "Manaus", "Brasil", "Encontro das Águas", -3.1190000000000002, -60.021700000000003, 2, 5.0, 1, 2500.0 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 30, new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Utc), "DRONE-002", 3, 1, 1, 45.0, "defensivo", "Torna-se invisível na água", "Camuflagem Aquática", "Rio de Janeiro", "Brasil", "Pão de Açúcar", -22.9068, -43.172899999999998, 2, 10.0, 1, 2200.0 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 15, new DateTime(2025, 10, 25, 12, 0, 0, 0, DateTimeKind.Utc), "DRONE-003", 5, 2, 1, 55.0, "mobilidade", "Voa em velocidades extremas", "Voo Supersônico", "São Paulo", "Brasil", "Parque Ibirapuera", -23.5505, -46.633299999999998, 2, 8.0, 1, 3000.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drones");

            migrationBuilder.DropTable(
                name: "Patos");
        }
    }
}
