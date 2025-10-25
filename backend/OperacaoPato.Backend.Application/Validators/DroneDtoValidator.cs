using FluentValidation;
using OperacaoPato.Backend.Application.DTOs;

namespace OperacaoPato.Backend.Application.Validators
{
    public class DroneDtoValidator : AbstractValidator<DroneDto>
    {
        public DroneDtoValidator()
        {
            RuleFor(x => x.NumeroSerie)
                .NotEmpty().WithMessage("Número de série é obrigatório")
                .MaximumLength(50).WithMessage("Número de série não pode exceder 50 caracteres");

            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("Marca é obrigatória")
                .MaximumLength(100).WithMessage("Marca não pode exceder 100 caracteres");

            RuleFor(x => x.Fabricante)
                .NotEmpty().WithMessage("Fabricante é obrigatório")
                .MaximumLength(100).WithMessage("Fabricante não pode exceder 100 caracteres");

            RuleFor(x => x.PaisOrigem)
                .NotEmpty().WithMessage("País de origem é obrigatório")
                .MaximumLength(100).WithMessage("País de origem não pode exceder 100 caracteres");
        }
    }
}