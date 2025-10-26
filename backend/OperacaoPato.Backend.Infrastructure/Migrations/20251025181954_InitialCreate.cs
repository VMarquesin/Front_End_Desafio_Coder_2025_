using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OperacaoPato.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    NumeroSerie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                columns: new[] { "NumeroSerie", "Fabricante", "Marca", "PaisOrigem" },
                values: new object[,]
                {
                    { "DRONE-001", "DJI Technology", "DJI", "China" },
                    { "DRONE-002", "Parrot SA", "Parrot", "França" },
                    { "DRONE-003", "Skydio Inc", "Skydio", "Estados Unidos" },
                    { "DRONE-004", "Autel Robotics", "Autel", "China" },
                    { "DRONE-005", "PowerVision Tech", "PowerVision", "China" }
                });

            migrationBuilder.InsertData(
                table: "Patos",
                columns: new[] { "Id", "DroneNumeroSerie", "Altura_Valor", "Altura_Unidade", "Peso_Valor", "Peso_Unidade", "Localizacao_Cidade", "Localizacao_Pais", "Localizacao_Latitude", "Localizacao_Longitude", "Localizacao_Precisao_Valor", "Localizacao_Precisao_Unidade", "Localizacao_PontoReferencia", "Status", "BatimentosPorMinuto", "QuantidadeMutacoes", "Poder_Nome", "Poder_Descricao", "Poder_Classificacao", "DataColetaUtc" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "DRONE-001", 50.0, 1, 2500.0, 0, "Manaus", "Brasil", -3.1190, -60.0217, 5.0, 0, "Encontro das Águas", 0, null, 2, "Rajada de Água", "Dispara jatos de água pressurizada", "ofensivo", new DateTime(2025, 10, 23, 12, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "DRONE-002", 45.0, 1, 2200.0, 0, "Rio de Janeiro", "Brasil", -22.9068, -43.1729, 10.0, 0, "Pão de Açúcar", 1, 30, 3, "Camuflagem Aquática", "Torna-se invisível na água", "defensivo", new DateTime(2025, 10, 24, 12, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "DRONE-003", 55.0, 1, 3000.0, 0, "São Paulo", "Brasil", -23.5505, -46.6333, 8.0, 0, "Parque Ibirapuera", 2, 15, 5, "Voo Supersônico", "Voa em velocidades extremas", "mobilidade", new DateTime(2025, 10, 25, 12, 0, 0, DateTimeKind.Utc) }
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
