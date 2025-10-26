using FluentValidation;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Application.Services;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Application.Services
{
    public class DroneService : IDroneService
    {
        private readonly IDroneRepository _droneRepository;
        private readonly IValidator<DroneDto> _droneDtoValidator;

        public DroneService(
            IDroneRepository droneRepository,
            IValidator<DroneDto> droneDtoValidator)
        {
            _droneRepository = droneRepository;
            _droneDtoValidator = droneDtoValidator;
        }

        // Novo: buscar todos drones (retorna DTOs)
        public async Task<IEnumerable<DroneDto>> ObterTodosAsync()
        {
            var entities = await _droneRepository.ObterTodosAsync();
            return entities.Select(MapToDto);
        }

        // Novo: cadastro unitário retornando DTO (usado pelo Controller)
        public async Task<DroneDto> CadastrarAsync(DroneDto droneDto)
        {
            var entity = await CreateInternalAsync(droneDto);
            return MapToDto(entity);
        }

        // Novo: cadastro em lote retornando DTOs (usado pelo Controller)
        public async Task<IEnumerable<DroneDto>> CadastrarLoteAsync(IEnumerable<DroneDto> dtos)
        {
            var result = new List<DroneDto>();
            foreach (var dto in dtos)
            {
                var created = await CadastrarAsync(dto);
                result.Add(created);
            }
            return result;
        }

        public async Task<bool> ApagarDroneAsync(string numeroSerie)
        {
            if (string.IsNullOrWhiteSpace(numeroSerie))
                throw new ArgumentException("Número de série é obrigatório", nameof(numeroSerie));

            var existente = await _droneRepository.ExisteAsync(numeroSerie);
            if (!existente)
                return false;

            return await _droneRepository.RemoverAsync(numeroSerie);
        }

        // Pipeline de criação compartilhado
        private async Task<Drone> CreateInternalAsync(DroneDto droneDto)
        {
            var validationResult = await _droneDtoValidator.ValidateAsync(droneDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existente = await _droneRepository.ExisteAsync(droneDto.NumeroSerie);
            if (existente)
                throw new InvalidOperationException($"Já existe um drone com número de série {droneDto.NumeroSerie}");

            // Criar níveis de recursos padrão
            var bateria = new NivelRecurso(100, 100, "%");
            var combustivel = new NivelRecurso(100, 100, "%");
            var integridade = new NivelRecurso(100, 100, "%");
            var posicaoInicial = new Coordenada(0, 0);

            var drone = new DroneOperacional(
                droneDto.NumeroSerie,
                droneDto.Marca,
                droneDto.Fabricante,
                droneDto.PaisOrigem,
                bateria,
                combustivel,
                integridade,
                posicaoInicial);

            return await _droneRepository.AdicionarAsync(drone);
        }

        private static DroneDto MapToDto(Drone entity)
        {
            return new DroneDto
            {
                NumeroSerie = entity.NumeroSerie,
                Marca = entity.Marca,
                Fabricante = entity.Fabricante,
                PaisOrigem = entity.PaisOrigem
            };
        }
    }
}