using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OperacaoPato.Backend.Infrastructure.Data;

namespace OperacaoPato.Backend.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real DbContext registration
            var descriptor = services.SingleOrDefault(d => 
                d.ServiceType == typeof(DbContextOptions<OperacaoPatoContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            services.AddDbContext<OperacaoPatoContext>(options =>
            {
                options.UseInMemoryDatabase("OperacaoPatoTestDb");
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<OperacaoPatoContext>();

            // Ensure the database is created
            db.Database.EnsureCreated();

            // Seed test data
            SeedTestData(db);
        });
    }

    private void SeedTestData(OperacaoPatoContext context)
    {
        // Add test drones
        var testDrone = new OperacaoPato.Backend.Domain.Entities.Drone(
            numeroSerie: "DRONE-TEST",
            marca: "TestDrone",
            fabricante: "Test Inc",
            paisOrigem: "Test Country");

        context.Drones.Add(testDrone);

        // Add test pato with known GUID for testing
        var testGuid = new Guid("33333333-3333-3333-3333-333333333333");
        var coordenada = new OperacaoPato.Backend.Domain.ValueObjects.Coordenada(-23.55052, -46.633308);
        var precisao = new OperacaoPato.Backend.Domain.ValueObjects.Comprimento(5.0, Domain.Enums.UnidadeComprimento.Metro);
        var local = new OperacaoPato.Backend.Domain.ValueObjects.Localizacao(
            "São Paulo", "Brasil", coordenada, precisao, "Parque Ibirapuera");
        var altura = new OperacaoPato.Backend.Domain.ValueObjects.Comprimento(55.0, Domain.Enums.UnidadeComprimento.Centimetro);
        var peso = new OperacaoPato.Backend.Domain.ValueObjects.Massa(3000.0, Domain.Enums.UnidadeMassa.Grama);
        var poder = new OperacaoPato.Backend.Domain.Entities.SuperPoder(
            "Voo Supersônico", "Voa muito rápido", "mobilidade");

        var testPato = new OperacaoPato.Backend.Domain.Entities.PatoPrimordial(
            id: testGuid,
            droneNumeroSerie: "DRONE-TEST",
            altura: altura,
            peso: peso,
            localizacao: local,
            status: Domain.Enums.StatusHibernacao.Transe,
            batimentosPorMinuto: 45,
            quantidadeMutacoes: 2,
            poder: poder,
            dataColetaUtc: DateTime.UtcNow);

        context.Patos.Add(testPato);
        context.SaveChanges();
    }
}