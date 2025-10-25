using FluentValidation;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Services;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Infrastructure.Repositories;
using OperacaoPato.Backend.Application.Validators;
using OperacaoPato.Backend.Application.UseCases.CadastrarDrone;
using OperacaoPato.Backend.Application.UseCases.CadastrarPato;
using OperacaoPato.Backend.Application.UseCases.ObterTodosPatos;
using OperacaoPato.Backend.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Validators (já existente)
builder.Services.AddScoped<IValidator<PatoDto>, PatoDtoValidator>();

// Use Cases
builder.Services.AddScoped<CadastrarDroneUseCase>();
builder.Services.AddScoped<ICadastrarPatoUseCase, CadastrarPatoUseCase>();
builder.Services.AddScoped<IObterTodosPatosUseCase, ObterTodosPatosUseCase>();

// Infra: Repository em memória
builder.Services.AddSingleton<IDroneRepository, InMemoryDroneRepository>();
builder.Services.AddSingleton<IPatoRepository, InMemoryPatoRepository>();

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
