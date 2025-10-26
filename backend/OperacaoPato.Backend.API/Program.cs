using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Services;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Infrastructure.Repositories;
using OperacaoPato.Backend.Application.Validators;
using OperacaoPato.Backend.Application.UseCases.CadastrarDrone;
using OperacaoPato.Backend.Application.UseCases.CadastrarPato;
using OperacaoPato.Backend.Application.UseCases.ObterTodosPatos;
using OperacaoPato.Backend.API.Filters;
using OperacaoPato.Backend.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrando serviços do DroneOperacional
builder.Services.AddScoped<OperacaoPato.Backend.Application.Services.ControladorVooService>();
builder.Services.AddScoped<OperacaoPato.Backend.Application.Services.NavegacaoService>();
builder.Services.AddScoped<OperacaoPato.Backend.Application.Services.MonitorStatusService>();
builder.Services.AddScoped<OperacaoPato.Backend.Application.Services.AnalisadorVulnerabilidades>();
builder.Services.AddScoped<OperacaoPato.Backend.Application.Services.GeradorDefesasService>();
builder.Services.AddScoped<OperacaoPato.Backend.Application.Services.TaticaAtaqueService>();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

// Application Services
builder.Services.AddScoped<IDroneService, DroneService>();
builder.Services.AddScoped<IValidator<DroneDto>, DroneDtoValidator>();

// Capture assessment service
builder.Services.AddScoped<CaptureAssessmentService>();

// Validators (já existente)
builder.Services.AddScoped<IValidator<PatoDto>, PatoDtoValidator>();

// Use Cases
builder.Services.AddScoped<CadastrarDroneUseCase>();
builder.Services.AddScoped<ICadastrarPatoUseCase, CadastrarPatoUseCase>();
builder.Services.AddScoped<IObterTodosPatosUseCase, ObterTodosPatosUseCase>();

// Entity Framework
builder.Services.AddDbContext<OperacaoPatoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IDroneRepository, EFDroneRepository>();
builder.Services.AddScoped<IPatoRepository, EFPatoRepository>();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.MapControllers();

app.Run();
