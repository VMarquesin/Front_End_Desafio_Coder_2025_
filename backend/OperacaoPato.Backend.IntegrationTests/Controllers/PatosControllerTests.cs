using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OperacaoPato.Backend.Application.DTOs;
using Xunit;

namespace OperacaoPato.Backend.IntegrationTests.Controllers;

public class PatosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public PatosControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task AssessPato_WithValidId_ReturnsAssessment()
    {
        // Arrange
        var patoId = new Guid("33333333-3333-3333-3333-333333333333"); // ID do pato de teste

        // Act
        var response = await _client.PostAsync($"/api/patos/{patoId}/assess", null);
        var result = await response.Content.ReadFromJsonAsync<CaptureAssessmentResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.PatoId.Should().Be(patoId);
        result.OperationalCostScore.Should().BeInRange(0, 100);
        result.CaptureRiskScore.Should().BeInRange(0, 100);
        result.ScientificValueScore.Should().BeInRange(0, 100);
        result.RequiredForceLevel.Should().NotBeNullOrWhiteSpace();
        result.Recommendation.Should().NotBeNullOrWhiteSpace();
        result.EstimatedTransportKm.Should().BeGreaterOrEqualTo(0);
        result.EstimatedTransportCost.Should().BeGreaterThan(0);
        result.EstimatedSequencingCost.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AssessPato_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/api/patos/{invalidId}/assess", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}