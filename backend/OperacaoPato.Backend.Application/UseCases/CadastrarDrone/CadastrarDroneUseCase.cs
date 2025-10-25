using FluentValidation;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Application.Interfaces;
using OperacaoPato.Backend.Shared.Exceptions;
using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Backend.Application.UseCases.CadastrarDrone
{
    public class CadastrarDroneUseCase
    {
        private readonly IDroneRepository _repo;
        private readonly IValidator<DroneDto> _validator;

        public CadastrarDroneUseCase(IDroneRepository repo, IValidator<DroneDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<DroneDto> HandleAsync(DroneDto input)
        {
            Validate(input);

            var exists = await _repo.ExisteAsync(input.NumeroSerie);
            if (exists)
                throw new InvalidOperationException($"Já existe um drone com número de série {input.NumeroSerie}");

            var entity = new Drone(input.NumeroSerie, input.Marca, input.Fabricante, input.PaisOrigem);
            var created = await _repo.AdicionarAsync(entity);

            return new DroneDto
            {
                NumeroSerie = created.NumeroSerie,
                Marca = created.Marca,
                Fabricante = created.Fabricante,
                PaisOrigem = created.PaisOrigem
            };
        }

        private void Validate(DroneDto input)
        {
            var result = _validator.Validate(input);
            if (!result.IsValid)
            {
                var errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorsMessages);
            }
        }
    }
} 